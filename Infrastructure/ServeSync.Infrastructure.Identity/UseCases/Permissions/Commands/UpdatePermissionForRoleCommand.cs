using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Commands;

public class UpdatePermissionForRoleCommand : ICommand<IEnumerable<PermissionDto>>
{
    public string RoleId { get; set; }
    public IEnumerable<Guid> PermissionIds { get; set; }
    
    public UpdatePermissionForRoleCommand(string roleId, IEnumerable<Guid> permissionIds)
    {
        RoleId = roleId;
        PermissionIds = permissionIds;
    }
}