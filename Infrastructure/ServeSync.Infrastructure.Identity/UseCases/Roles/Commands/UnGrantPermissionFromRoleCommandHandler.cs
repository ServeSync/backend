using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Commands;

public class UnGrantPermissionFromRoleCommandHandler : ICommandHandler<UnGrantPermissionFromRoleCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnGrantPermissionFromRoleCommandHandler(
        IRoleRepository roleRepository, 
        IPermissionRepository permissionRepository, 
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(UnGrantPermissionFromRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new RoleNotFoundException(request.RoleId);
        }

        var permission = await _permissionRepository.FindByIdAsync(request.PermissionId);
        if (permission == null)
        {
            throw new PermissionNotFoundException(request.PermissionId);
        }
        
        role.UnGrantPermission(permission);
        
        _roleRepository.Update(role);
        await _unitOfWork.CommitAsync();
    }
}