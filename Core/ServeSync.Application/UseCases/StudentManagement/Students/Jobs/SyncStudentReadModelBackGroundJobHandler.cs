using Microsoft.Extensions.Logging;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Jobs;

public class SyncStudentReadModelBackGroundJobHandler : IBackGroundJobHandler<SyncStudentReadModelBackGroundJob>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly ILogger<SyncStudentReadModelBackGroundJobHandler> _logger;
    
    public SyncStudentReadModelBackGroundJobHandler(
        IStudentRepository studentRepository,
        IEventReadModelRepository eventReadModelRepository,
        ILogger<SyncStudentReadModelBackGroundJobHandler> logger)
    {
        _studentRepository = studentRepository;
        _eventReadModelRepository = eventReadModelRepository;
        _logger = logger;
    }
    
    public async Task Handle(SyncStudentReadModelBackGroundJob notification, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindByIdAsync(notification.StudentId, nameof(Student.HomeRoom));
        if (student == null)
        {
            _logger.LogError("Sync student read model failed: student '{Id}' not found", notification.StudentId);
            throw new StudentNotFoundException(notification.StudentId);
        }
        
        await _eventReadModelRepository.UpdateStudentInEventsAsync(student);
        _logger.LogInformation("Sync student read model success: student '{Id}'", notification.StudentId);
    }
}