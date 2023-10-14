using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;

public class CreateEventCollaborationCommand : ICommand<Guid>
{
    public EventCollaborationRequestDto EventCollaborationRequest { get; set;}

    public CreateEventCollaborationCommand(EventCollaborationRequestDto eventCollaborationRequestDto)
    {
        EventCollaborationRequest = eventCollaborationRequestDto; 
    }    
}
