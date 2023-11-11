using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;

public class EventRejectedDomainEvent : IDomainEvent
{
    public Event Event { get; set; }
    
    public EventRejectedDomainEvent(Event @event)
    {
        Event = @event;
    }
}