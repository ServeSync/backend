using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class NewPendingOrganizationContactCreatedDomainEvent : IDomainEvent
{
    public EventOrganizationContact Contact { get; set; }
    
    public NewPendingOrganizationContactCreatedDomainEvent(EventOrganizationContact contact)
    {
        Contact = contact;
    }
}