using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Specifications;
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
        await SeedPermissionForStudentRoleAsync();
        await SeedPermissionForStudentAffairRoleAsync();
        await SeedPermissionForEventOrganizerRoleAsync();
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
    
    private async Task SeedPermissionForStudentAffairRoleAsync()
    {
        _logger.LogInformation("Begin seeding not granted permissions for student affair...");

        var studentAffair = await _roleRepository.FindByNameAsync(AppRole.StudentAffair);
        if (studentAffair == null)
        {
            _logger.LogWarning("Student affair role not found.");
            return;
        }
        
        var permissions = await _permissionRepository.FilterAsync(new PermissionByIncludedNameSpecification(StudentAffairPermissions.Provider));
        var notGrantedPermissions = permissions.ExceptBy(studentAffair.Permissions.Select(x => x.PermissionId), x => x.Id);

        if (!notGrantedPermissions.Any())
        {
            _logger.LogInformation("Student affair has all permissions.");
            return;   
        }
        
        foreach (var permission in notGrantedPermissions)
        {
            studentAffair.GrantPermission(permission.Id);
        }

        _roleRepository.Update(studentAffair);
        await _unitOfWork.CommitAsync();
    }

    private async Task SeedPermissionForStudentRoleAsync()
    {
        _logger.LogInformation("Begin seeding not granted permissions for admin...");

        var studentRole = await _roleRepository.FindByNameAsync(AppRole.Student);
        if (studentRole == null)
        {
            _logger.LogWarning("Student role not found.");
            return;
        }
        
        var permissions = await _permissionRepository.FilterAsync(new PermissionByIncludedNameSpecification(StudentPermissions.Provider));
        var notGrantedPermissions = permissions.ExceptBy(studentRole.Permissions.Select(x => x.PermissionId), x => x.Id);

        if (!notGrantedPermissions.Any())
        {
            _logger.LogInformation("Student role has all permissions.");
            return;   
        }
        
        foreach (var permission in notGrantedPermissions)
        {
            studentRole.GrantPermission(permission.Id);
        }

        _roleRepository.Update(studentRole);
        await _unitOfWork.CommitAsync();
    }
    
    private async Task SeedPermissionForEventOrganizerRoleAsync()
    {
        _logger.LogInformation("Begin seeding not granted permissions for event organizer...");

        var eventOrganizerRole = await _roleRepository.FindByNameAsync(AppRole.EventOrganizer);
        if (eventOrganizerRole == null)
        {
            _logger.LogWarning("Event organizer role not found.");
            return;
        }
        
        var permissions = await _permissionRepository.FilterAsync(new PermissionByIncludedNameSpecification(EventOrganizerPermissions.Provider));
        var notGrantedPermissions = permissions.ExceptBy(eventOrganizerRole.Permissions.Select(x => x.PermissionId), x => x.Id);

        if (!notGrantedPermissions.Any())
        {
            _logger.LogInformation("Admin role has all permissions.");
            return;   
        }
        
        foreach (var permission in notGrantedPermissions)
        {
            eventOrganizerRole.GrantPermission(permission.Id);
        }

        _roleRepository.Update(eventOrganizerRole);
        await _unitOfWork.CommitAsync();
    }
}