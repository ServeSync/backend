using MediatR;

namespace ServeSync.Domain.SeedWorks.Events;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event);
}