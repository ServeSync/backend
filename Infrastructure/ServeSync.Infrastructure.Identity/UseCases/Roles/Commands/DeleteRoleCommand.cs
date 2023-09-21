using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Commands;

public class DeleteRoleCommand : ICommand
{
    public string Id { get; set; }

    public DeleteRoleCommand(string id)
    {
        Id = id;
    }
}