using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand, Guid>
{
    private readonly IStudentDomainService _studentDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateStudentCommandHandler> _logger;
    
    public CreateStudentCommandHandler(
        IStudentDomainService studentDomainService,
        IUnitOfWork unitOfWork,
        ILogger<CreateStudentCommandHandler> logger)
    {
        _studentDomainService = studentDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Guid> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentDomainService.CreateAsync(
            request.Code, 
            request.FullName, 
            request.Gender,
            request.Birth, 
            request.ImageUrl, 
            request.CitizenId, 
            request.Email, 
            request.Phone, 
            request.HomeRoomId,
            request.EducationProgramId, 
            request.HomeTown, 
            request.Address);

        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Created new student with id {StudentId}", student.Id);
        
        return student.Id;
    }
}