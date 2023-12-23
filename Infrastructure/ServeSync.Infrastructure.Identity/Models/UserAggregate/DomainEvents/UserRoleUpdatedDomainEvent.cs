using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.DomainEvents;

public record UserRoleUpdatedDomainEvent : EquatableDomainEvent
{
    public string UserId { get; set; }
    public Guid TenantId { get; set; }

    public UserRoleUpdatedDomainEvent(string userId, Guid tenantId)
    {
        UserId = userId;
        TenantId = tenantId;
    }
}