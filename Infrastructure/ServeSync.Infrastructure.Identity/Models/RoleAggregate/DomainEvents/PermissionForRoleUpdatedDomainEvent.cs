using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.DomainEvents;

public record PermissionForRoleUpdatedDomainEvent : EquatableDomainEvent
{
    public string Name { get; }
    
    public PermissionForRoleUpdatedDomainEvent(string name)
    {
        Name = name;
    }
}