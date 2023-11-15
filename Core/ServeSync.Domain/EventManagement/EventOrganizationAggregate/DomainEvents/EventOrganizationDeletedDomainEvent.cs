using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class EventOrganizationDeletedDomainEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public string IdentityId { get; set; }
    
    public EventOrganizationDeletedDomainEvent(Guid id, string identityId)
    {
        Id = id;
        IdentityId = identityId;
    }
}