using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;

public record EventUpdatedDomainEvent : EquatableDomainEvent
{
    public Guid EventId { get; set; } 
    
    public EventUpdatedDomainEvent(Guid eventId)
    {
        EventId = eventId;
    }
}