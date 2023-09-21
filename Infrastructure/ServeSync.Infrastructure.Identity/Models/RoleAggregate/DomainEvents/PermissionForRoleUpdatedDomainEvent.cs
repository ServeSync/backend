using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.DomainEvents;

public record PermissionForRoleUpdatedDomainEvent : EquatableDomainEvent
{
    public string RoleId { get; }
    
    public PermissionForRoleUpdatedDomainEvent(string roleId)
    {
        RoleId = roleId;
    }
}