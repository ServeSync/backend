using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

public partial class ApplicationRole : IAggregateRoot<string>
{
    private List<IDomainEvent> _domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();
        
    public void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents = _domainEvents ?? new List<IDomainEvent>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}