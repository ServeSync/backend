using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Commands;

public class CreateRoleCommand : ICommand<RoleDto>
{
    public string Name { get; set; }

    public CreateRoleCommand(string name)
    {
        Name = name;
    }
}