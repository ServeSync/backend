using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;

public record NewEventAttendanceInfoCreatedDomainEvent : EquatableDomainEvent
{
    public Guid EventId { get; set; }
    
    public NewEventAttendanceInfoCreatedDomainEvent(Guid eventId)
    {
        EventId = eventId;
    }
}