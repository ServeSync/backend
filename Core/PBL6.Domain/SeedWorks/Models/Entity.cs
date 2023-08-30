using PBL6.Domain.SeedWorks.Events;

namespace PBL6.Domain.SeedWorks.Models;

public abstract class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; }
    
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

public abstract class Entity : Entity<Guid>
{
    
}