using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Queries;

public class GetRoleByIdQuery : IQuery<RoleDto>
{
    public string Id { get; set; }

    public GetRoleByIdQuery(string id)
    {
        Id = id;
    }
}