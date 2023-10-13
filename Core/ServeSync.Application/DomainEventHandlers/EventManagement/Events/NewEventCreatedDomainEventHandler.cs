using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.Events;

public class NewEventCreatedDomainEventHandler : IDomainEventHandler<NewEventCreatedDomainEvent>
{
    public Task Handle(NewEventCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Todo: Send email to students
        
        return Task.CompletedTask;
    }
}