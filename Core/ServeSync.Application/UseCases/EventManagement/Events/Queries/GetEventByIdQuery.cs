using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetEventByIdQuery : IQuery<EventDetailDto>
{
    public Guid EventId { get; set; }
    
    public GetEventByIdQuery(Guid eventId)
    {
        EventId = eventId;
    }
}