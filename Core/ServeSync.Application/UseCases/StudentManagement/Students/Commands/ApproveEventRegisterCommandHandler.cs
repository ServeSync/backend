using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class ApproveEventRegisterCommandHandler : ICommandHandler<ApproveEventRegisterCommand>
{
    private readonly IStudentDomainService _studentDomainService;
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveEventRegisterCommandHandler> _logger;
    
    public ApproveEventRegisterCommandHandler(
        IStudentDomainService studentDomainService,
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork,
        ILogger<ApproveEventRegisterCommandHandler> logger)
    {
        _studentDomainService = studentDomainService;
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(ApproveEventRegisterCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindByIdAsync(request.StudentId);
        if (student == null)
        {
            throw new StudentNotFoundException(request.StudentId);
        }
        
        await _studentDomainService.ApproveEventRegisterAsync(student, request.EventRegisterId, DateTime.UtcNow);
        _studentRepository.Update(student);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Student {StudentId} has been approved to EventRegister {EventRegisterId}", request.StudentId, request.EventRegisterId);
    }
}