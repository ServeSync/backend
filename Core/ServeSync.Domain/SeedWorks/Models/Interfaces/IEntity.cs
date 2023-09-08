using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface IEntity<out TKey> where TKey : IEquatable<TKey>
{
    TKey Id { get; }

    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void AddDomainEvent(IDomainEvent eventItem);

    void RemoveDomainEvent(IDomainEvent eventItem);

    void ClearDomainEvents();
}

public interface IEntity : IEntity<Guid>
{
}