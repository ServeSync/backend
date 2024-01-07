using Microsoft.Extensions.Logging;
using ServeSync.Application.QueryObjects;
using ServeSync.Application.ReadModels.Abstracts;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.EventManagement.Events.Jobs;
using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.Events;

public class SyncEventReadModelDomainEventHandler : 
    IPersistedDomainEventHandler<EventUpdatedDomainEvent>, 
    IPersistedDomainEventHandler<NewEventCreatedDomainEvent>,
    IPersistedDomainEventHandler<StudentEventRegisterApprovedDomainEvent>,
    IPersistedDomainEventHandler<StudentEventRegisterRejectedDomainEvent>,
    IPersistedDomainEventHandler<StudentAttendedToEventDomainEvent>,
    IPersistedDomainEventHandler<StudentRegisteredToEventRoleDomainEvent>
{
    private readonly IBackGroundJobManager _backGroundJobManager;
    private readonly IEventQueries _eventQueries;
    private readonly IReadModelRepository<EventReadModel, Guid> _eventReadModelRepository;
    private readonly IBasicReadOnlyRepository<EventRole, Guid> _eventRoleRepository;
    private readonly ILogger<SyncEventReadModelDomainEventHandler> _logger;
    
    public SyncEventReadModelDomainEventHandler(
        IBackGroundJobManager backGroundJobManager,
        IEventQueries eventQueries,
        IReadModelRepository<EventReadModel, Guid> eventReadModelRepository,
        IBasicReadOnlyRepository<EventRole, Guid> eventRoleRepository,
        ILogger<SyncEventReadModelDomainEventHandler> logger)
    {
        _eventQueries = eventQueries;
        _eventReadModelRepository = eventReadModelRepository;
        _backGroundJobManager = backGroundJobManager;
        _eventRoleRepository = eventRoleRepository;
        _logger = logger;
    }
    
    public async Task Handle(EventUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await SyncEventData(notification.EventId);
    }

    public async Task Handle(NewEventCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await SyncEventData(notification.Event.Id);
    }

    public async Task Handle(StudentEventRegisterApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        await SyncEventDataByEventRoleIdAsync(notification.EventRoleId);
    }
    
    public async Task Handle(StudentEventRegisterRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        await SyncEventDataByEventRoleIdAsync(notification.EventRoleId);
    }
    
    public async Task Handle(StudentAttendedToEventDomainEvent notification, CancellationToken cancellationToken)
    {
        await SyncEventDataByEventRoleIdAsync(notification.EventRoleId);
    }
    
    public async Task Handle(StudentRegisteredToEventRoleDomainEvent notification, CancellationToken cancellationToken)
    {
        await SyncEventDataByEventRoleIdAsync(notification.EventRoleId);
    }
    
    private async Task SyncEventData(Guid id)
    {
        var @event = await _eventQueries.GetEventReadModelByIdAsync(id);
        if (@event != null)
        {
            await _eventReadModelRepository.CreateOrUpdateAsync(@event);
                
            _logger.LogInformation("Sync data for event '{EventId}' success", id);
        }
    }
    
    private void DelaySyncEventData(Guid id)
    {
        var job = new SyncEventReadModelBackGroundJob(id);
        _backGroundJobManager.Fire(job);
    }

    private async Task SyncEventDataByEventRoleIdAsync(Guid eventRoleId)
    {
        var eventRole = await _eventRoleRepository.FindByIdAsync(eventRoleId);
        if (eventRole == null)
        {
            throw new EventRoleNotFoundException(eventRoleId);
        }
        
        await SyncEventData(eventRole.EventId);
    }
}