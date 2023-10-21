using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.EventManagement.Events.Jobs;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class StudentRegisteredToEventRoleDomainEventHandler : IDomainEventHandler<StudentRegisteredToEventRoleDomainEvent>
{
    private readonly IBasicReadOnlyRepository<EventRole, Guid> _eventRoleRepository;
    private readonly IBackGroundJobManager _backGroundJobManager;
    
    public StudentRegisteredToEventRoleDomainEventHandler(
        IBasicReadOnlyRepository<EventRole, Guid> eventRoleRepository,
        IBackGroundJobManager backGroundJobManager)
    {
        _eventRoleRepository = eventRoleRepository;
        _backGroundJobManager = backGroundJobManager;
    }
    
    public async Task Handle(StudentRegisteredToEventRoleDomainEvent notification, CancellationToken cancellationToken)
    {
        var eventRole = await _eventRoleRepository.FindByIdAsync(notification.EventRoleId);
        if (eventRole == null)
        {
            throw new EventRoleNotFoundException(notification.EventRoleId);
        }

        var syncEventJob = new SyncEventReadModelBackGroundJob(eventRole.EventId);
        _backGroundJobManager.Fire(syncEventJob);
    }
}