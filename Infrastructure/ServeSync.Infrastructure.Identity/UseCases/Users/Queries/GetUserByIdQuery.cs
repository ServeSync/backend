using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

public class GetUserByIdQuery : IQuery<UserDetailDto>
{
    public string Id { get; set; }
    
    public GetUserByIdQuery(string id)
    {
        Id = id;
    }
    
}