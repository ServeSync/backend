using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class AttendEventCommandHandler : ICommandHandler<AttendEventCommand>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentDomainService _studentDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<AttendEventCommandHandler> _logger;
    
    public AttendEventCommandHandler(
        IStudentRepository studentRepository,
        IStudentDomainService studentDomainService,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        ILogger<AttendEventCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _studentDomainService = studentDomainService;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _logger = logger;
    }
    
    public async Task Handle(AttendEventCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindByIdentityAsync(_currentUser.Id);
        if (student == null)
        {
            throw new StudentNotFoundException(_currentUser.Id);
        }
        
        await _studentDomainService.AttendEventAsync(student, request.EventId, request.Code, DateTime.Now);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Student with identity Id {StudentId} attended event {EventId}", _currentUser.Id, request.EventId);
    }
}