using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

public class GetAllPermissionForRoleQuery : IQuery<IEnumerable<PermissionDto>>
{
    public string RoleId { get; set; }
    public string Name { get; set; }
    
    public GetAllPermissionForRoleQuery(string roleId, string name)
    {
        RoleId = roleId;
        Name = name;
    }
}