using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;

public class CreateEventCollaborationCommand : ICommand<Guid>
{
    public EventCollaborationRequestCreateDto EventCollaborationCreateRequest { get; set;}

    public CreateEventCollaborationCommand(EventCollaborationRequestCreateDto eventCollaborationRequestCreateDto)
    {
        EventCollaborationCreateRequest = eventCollaborationRequestCreateDto; 
    }    
}
