using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

public class GetRolesForUserQuery : IQuery<List<RoleDto>>
{
    public string UserId { get; set; }
    public Guid TenantId { get; set; }
    
    public GetRolesForUserQuery(string userId, Guid tenantId)
    {
        UserId = userId;
        TenantId = tenantId;
    }
}