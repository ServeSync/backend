using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

public class GetAllPermissionForUserQuery : IQuery<IEnumerable<PermissionDto>>
{
    public string UserId { get; set; }
    
    public GetAllPermissionForUserQuery(string userId)
    {
        UserId = userId;
    }
}