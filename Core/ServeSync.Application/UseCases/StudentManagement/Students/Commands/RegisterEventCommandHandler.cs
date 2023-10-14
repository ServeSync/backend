using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class RegisterEventCommandHandler : ICommandHandler<RegisterEventCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentDomainService _studentDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterEventCommandHandler> _logger;
    
    public RegisterEventCommandHandler(
        ICurrentUser currentUser,
        IStudentRepository studentRepository,
        IStudentDomainService studentDomainService,
        IUnitOfWork unitOfWork,
        ILogger<RegisterEventCommandHandler> logger)
    {
        _currentUser = currentUser;
        _studentRepository = studentRepository;
        _studentDomainService = studentDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(RegisterEventCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindByIdentityAsync(_currentUser.Id);
        if (student == null)
        {
            throw new StudentNotFoundException(_currentUser.Id);
        }

        await _studentDomainService.RegisterEvent(student, request.EventRoleId, request.Description);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Student {StudentId} registered event role {EventRoleId}", _currentUser.Id, request.EventRoleId);
    }
}