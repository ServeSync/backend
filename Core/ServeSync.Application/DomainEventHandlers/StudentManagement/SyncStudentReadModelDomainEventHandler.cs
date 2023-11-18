using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.StudentManagement.Students.Jobs;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class SyncStudentReadModelDomainEventHandler : 
    IPersistedDomainEventHandler<StudentCodeUpdatedDomainEvent>,
    IPersistedDomainEventHandler<StudentContactInfoUpdatedDomainEvent>
{
    private readonly IBackGroundJobManager _backGroundJobManager;
    private ILogger<SyncStudentReadModelDomainEventHandler> _logger;
    
    public SyncStudentReadModelDomainEventHandler(
        IBackGroundJobManager backGroundJobManager,
        ILogger<SyncStudentReadModelDomainEventHandler> logger)
    {
        _backGroundJobManager = backGroundJobManager;
        _logger = logger;
    }
    
    public Task Handle(StudentCodeUpdatedDomainEvent @event, CancellationToken cancellationToken)
    {
        SyncStudentReadModel(@event.Id);
        return Task.CompletedTask;
    }

    public Task Handle(StudentContactInfoUpdatedDomainEvent @event, CancellationToken cancellationToken)
    {
        SyncStudentReadModel(@event.Id);
        return Task.CompletedTask;
    }
    
    private void SyncStudentReadModel(Guid studentId)
    {
        var job = new SyncStudentReadModelBackGroundJob(studentId);
        _backGroundJobManager.Fire(job);
    }
}