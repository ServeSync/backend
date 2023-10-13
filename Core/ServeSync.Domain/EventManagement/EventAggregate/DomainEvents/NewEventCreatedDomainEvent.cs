using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;

public class NewEventCreatedDomainEvent : IDomainEvent
{
    public Event Event { get; }
    
    public NewEventCreatedDomainEvent(Event @event)
    {
        Event = @event;
    }
}