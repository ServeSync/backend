using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

public class GetAllPermissionForUserQuery : IQuery<IEnumerable<PermissionDto>>
{
    public string UserId { get; set; }
    public Guid TenantId { get; set; }

    public GetAllPermissionForUserQuery(string userId, Guid tenantId)
    {
        UserId = userId;
        TenantId = tenantId;
    }
}