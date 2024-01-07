using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.EventManagement.Events.Jobs;
using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.Events;

public class NewEventCreatedDomainEventHandler : IDomainEventHandler<NewEventAttendanceInfoCreatedDomainEvent>
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
    
    public Task Handle(NewEventAttendanceInfoCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        var job = new GenerateAttendanceQrCodeBackGroundJob(@event.EventId);
        _backGroundJobManager.Fire(job);
        
        return Task.CompletedTask;
    }
}