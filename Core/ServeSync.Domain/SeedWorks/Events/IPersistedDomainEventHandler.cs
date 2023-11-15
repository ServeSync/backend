namespace ServeSync.Domain.SeedWorks.Events;

public interface IPersistedDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}