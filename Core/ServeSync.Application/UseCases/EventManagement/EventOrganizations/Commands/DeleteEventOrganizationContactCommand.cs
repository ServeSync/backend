using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class DeleteEventOrganizationContactCommand : ICommand
{
    public Guid EventOrganizationId { get; set; }
    
    public Guid EventOrganizationContactId { get; set; }
    
    public DeleteEventOrganizationContactCommand(Guid eventOrganizationId, Guid eventOrganizationContactId)
    {
        EventOrganizationId = eventOrganizationId;
        EventOrganizationContactId = eventOrganizationContactId;
    }
}