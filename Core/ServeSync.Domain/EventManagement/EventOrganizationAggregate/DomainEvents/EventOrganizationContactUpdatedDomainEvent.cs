using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class EventOrganizationContactUpdatedDomainEvent : IDomainEvent
{
    public EventOrganizationContact EventOrganizationContact { get; set; }
    
    public EventOrganizationContactUpdatedDomainEvent(EventOrganizationContact eventOrganizationContact)
    {
        EventOrganizationContact = eventOrganizationContact;
    }
}