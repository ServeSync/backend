using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class DeleteEventOrganizationCommand : ICommand
{
    public Guid Id { get; set; }

    public DeleteEventOrganizationCommand(Guid id)
    {
        Id = id;
    }
    
}