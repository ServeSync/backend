using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class EventOrganizationContactDeletedDomainEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public string IdentityId { get; set; }
    public Guid TenantId { get; set; }
    
    public EventOrganizationContactDeletedDomainEvent(Guid id, string identityId, Guid tenantId)
    {
        Id = id;
        IdentityId = identityId;
        TenantId = tenantId;
    }
}