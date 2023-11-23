using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class EventOrganizationContactDeletedDomainEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public OrganizationStatus Status { get; set; }
    public string IdentityId { get; set; }
    public Guid TenantId { get; set; }
    
    public EventOrganizationContactDeletedDomainEvent(Guid id, OrganizationStatus status, string identityId, Guid tenantId)
    {
        Id = id;
        Status = status;
        IdentityId = identityId;
        TenantId = tenantId;
    }
}