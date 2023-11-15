using Microsoft.Extensions.Logging;
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
    private readonly IBasicReadOnlyRepository<EventRole, Guid> _eventRoleRepository;
    private readonly ILogger<SyncEventReadModelDomainEventHandler> _logger;
    
    public SyncEventReadModelDomainEventHandler(
        IBackGroundJobManager backGroundJobManager,
        IBasicReadOnlyRepository<EventRole, Guid> eventRoleRepository,
        ILogger<SyncEventReadModelDomainEventHandler> logger)
    {
        _backGroundJobManager = backGroundJobManager;
        _eventRoleRepository = eventRoleRepository;
        _logger = logger;
    }
    
    public Task Handle(EventUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        SyncEventData(notification.EventId);
        return Task.CompletedTask;
    }

    public Task Handle(NewEventCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        SyncEventData(notification.Event.Id);
        return Task.CompletedTask;
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
    
    private void SyncEventData(Guid id)
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
        
        SyncEventData(eventRole.EventId);
    }
}