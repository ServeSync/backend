using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class EventOrganizationDeletedDomainEvent : IDomainEvent
{
    public EventOrganization EventOrganization { get; set; }
    
    public EventOrganizationDeletedDomainEvent(EventOrganization eventOrganization)
    {
        EventOrganization = eventOrganization;
    }
}