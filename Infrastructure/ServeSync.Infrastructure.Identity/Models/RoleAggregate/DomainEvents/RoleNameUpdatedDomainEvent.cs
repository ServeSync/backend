using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.DomainEvents;

public record RoleNameUpdatedDomainEvent : EquatableDomainEvent
{
    public string Name { get; }
    
    public RoleNameUpdatedDomainEvent(string name)
    {
        Name = name;
    }
}