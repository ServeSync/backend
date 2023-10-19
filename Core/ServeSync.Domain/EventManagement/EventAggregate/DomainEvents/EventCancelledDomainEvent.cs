using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;

public class EventCancelledDomainEvent : IDomainEvent
{
    public Event Event { get; }

    public EventCancelledDomainEvent(Event @event)
    {
        Event = @event;
    }
}