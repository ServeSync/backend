using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.EventManagement.Events.Jobs;
using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.Events;

public class EventUpdatedDomainEventHandler : IDomainEventHandler<EventUpdatedDomainEvent>
{
    private readonly IBackGroundJobManager _backGroundJobManager;
    
    public EventUpdatedDomainEventHandler(IBackGroundJobManager backGroundJobManager)
    {
        _backGroundJobManager = backGroundJobManager;
    }
    
    public Task Handle(EventUpdatedDomainEvent eventUpdated, CancellationToken cancellationToken)
    {
        var job = new SyncEventReadModelBackGroundJob(eventUpdated.EventId);
        _backGroundJobManager.Fire(job);;
        
        return Task.CompletedTask;
    }
}