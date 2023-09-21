using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Commands;

public class UpdateRoleCommand : ICommand<RoleDto>
{
    public string Id { get; set; }
    public string Name { get; set; }

    public UpdateRoleCommand(string id, string name)
    {
        Id = id;
        Name = name;
    }
}