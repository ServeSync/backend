using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface IDomainModel
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void AddDomainEvent(IDomainEvent eventItem);

    void RemoveDomainEvent(IDomainEvent eventItem);

    void ClearDomainEvents();
}