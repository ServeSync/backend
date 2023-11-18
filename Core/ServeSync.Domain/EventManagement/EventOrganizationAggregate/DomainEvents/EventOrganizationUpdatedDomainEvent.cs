using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class EventOrganizationUpdatedDomainEvent : IDomainEvent
{
    public EventOrganization EventOrganization { get; set; }
    
    public EventOrganizationUpdatedDomainEvent(EventOrganization eventOrganization)
    {
        EventOrganization = eventOrganization;
    }
}