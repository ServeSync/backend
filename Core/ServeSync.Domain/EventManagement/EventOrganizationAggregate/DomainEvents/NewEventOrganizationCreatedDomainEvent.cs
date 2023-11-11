using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class NewEventOrganizationCreatedDomainEvent : IDomainEvent
{
    public EventOrganization Organization { get; set; }
    
    public NewEventOrganizationCreatedDomainEvent(EventOrganization organization)
    {
        Organization = organization;
    }
}