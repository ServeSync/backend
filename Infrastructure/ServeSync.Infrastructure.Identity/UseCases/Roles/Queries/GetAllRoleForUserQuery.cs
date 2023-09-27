using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Queries;

public class GetAllRoleForUserQuery : IQuery<IEnumerable<string>>
{
    public string UserId { get; set; }
    
    public GetAllRoleForUserQuery(string userId)
    {
        UserId = userId;
    }
}