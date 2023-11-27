using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class NewPendingEventOrganizationCreatedDomainEvent : IDomainEvent
{
    public EventOrganization Organization { get; set; }
    
    public NewPendingEventOrganizationCreatedDomainEvent(EventOrganization organization)
    {
        Organization = organization;
    }
}