using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Queries;

public class GetEventCollaborationRequestByIdQuery : IQuery<EventCollaborationRequestDetailDto>
{
    public Guid EventCollaborationRequestId { get; set; }
        
    public GetEventCollaborationRequestByIdQuery(Guid eventCollaborationRequestId)
    {
        EventCollaborationRequestId = eventCollaborationRequestId;
    }
}
