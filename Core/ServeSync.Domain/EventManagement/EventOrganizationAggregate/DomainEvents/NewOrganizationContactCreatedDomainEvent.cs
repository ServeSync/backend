using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class NewOrganizationContactCreatedDomainEvent : IDomainEvent
{
    public EventOrganizationContact Contact { get; set; }
    
    public NewOrganizationContactCreatedDomainEvent(EventOrganizationContact contact)
    {
        Contact = contact;
    }
}