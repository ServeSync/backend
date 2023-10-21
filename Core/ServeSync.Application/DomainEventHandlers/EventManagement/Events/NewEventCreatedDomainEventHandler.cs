using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.EventManagement.Events.Jobs;
using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.Events;

public class NewEventCreatedDomainEventHandler : IDomainEventHandler<NewEventCreatedDomainEvent>
{
    private readonly IBackGroundJobManager _backGroundJobManager;
    private readonly ILogger<NewEventCreatedDomainEventHandler> _logger;
    
    public NewEventCreatedDomainEventHandler(
        IBackGroundJobManager backGroundJobManager,
        ILogger<NewEventCreatedDomainEventHandler> logger)
    {
        _backGroundJobManager = backGroundJobManager;
        _logger = logger;
    }
    
    public Task Handle(NewEventCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        SyncEventData(@event.Event);
        GenerateQrCode(@event.Event);
        
        return Task.CompletedTask;
    }

    private void GenerateQrCode(Event @event)
    {
        var job = new GenerateAttendanceQrCodeBackGroundJob(@event.Id);
        _backGroundJobManager.Fire(job);
    }
    
    private void SyncEventData(Event @event)
    {
        var job = new SyncEventReadModelBackGroundJob(@event.Id);
        _backGroundJobManager.Fire(job);
    }
}