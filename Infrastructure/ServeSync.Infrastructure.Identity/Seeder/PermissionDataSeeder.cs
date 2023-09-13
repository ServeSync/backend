using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;

namespace ServeSync.Infrastructure.Identity.Seeder;

public class PermissionDataSeeder : IDataSeeder
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PermissionDataSeeder> _logger;

    public PermissionDataSeeder(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork,
        ILogger<PermissionDataSeeder> logger)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SyncPermissionAsync();
        await SeedPermissionForAdminRoleAsync();
    }

    private async Task SyncPermissionAsync()
    {
        _logger.LogInformation("Begin syncing permissions...");

        var permissions = await _permissionRepository.FindAllAsync();

        var deletePermissions = permissions.ExceptBy(Permissions.Provider.Select(x => x.Name), x => x.Name);
        var newPermissions = Permissions.Provider.ExceptBy(permissions.Select(x => x.Name), x => x.Name);
        foreach (var permission in deletePermissions)
        {
            _permissionRepository.Delete(permission);
        }

        foreach (var permission in newPermissions)
        {
            await _permissionRepository.InsertAsync(new ApplicationPermission(permission.Name, permission.Description));
        }

        await _unitOfWork.CommitAsync();
    }

    private async Task SeedPermissionForAdminRoleAsync()
    {
        _logger.LogInformation("Begin seeding not granted permissions for admin...");

        var adminRole = await _roleRepository.FindByNameAsync(AppRole.Admin);
        if (adminRole == null)
        {
            _logger.LogWarning("Admin role not found.");
            return;
        }
        
        var permissions = await _permissionRepository.FindAllAsync();
        var notGrantedPermissions = permissions.ExceptBy(adminRole.Permissions.Select(x => x.PermissionId), x => x.Id);

        if (!notGrantedPermissions.Any())
        {
            _logger.LogInformation("Admin role has all permissions.");
            return;   
        }
        
        foreach (var permission in notGrantedPermissions)
        {
            adminRole.GrantPermission(permission.Id);
        }

        _roleRepository.Update(adminRole);
        await _unitOfWork.CommitAsync();
    }
}