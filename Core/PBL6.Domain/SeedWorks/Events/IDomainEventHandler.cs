using MediatR;

namespace PBL6.Domain.SeedWorks.Events;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event);
}