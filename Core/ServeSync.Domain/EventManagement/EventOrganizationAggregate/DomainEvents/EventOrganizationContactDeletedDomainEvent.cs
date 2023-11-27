using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class EventOrganizationContactDeletedDomainEvent : IDomainEvent
{
    public EventOrganizationContact EventOrganizationContact { get; set; }
    public Guid TenantId { get; set; }
    
    public EventOrganizationContactDeletedDomainEvent(EventOrganizationContact eventOrganizationContact, Guid tenantId)
    {
        EventOrganizationContact = eventOrganizationContact;
        TenantId = tenantId;
    }
}