using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class UpdateStudentCommandHandler : ICommandHandler<UpdateStudentCommand>
{
    private readonly IStudentDomainService _studentDomainService;
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateStudentCommandHandler> _logger;
    
    public UpdateStudentCommandHandler(
        IStudentDomainService studentDomainService, 
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateStudentCommandHandler> logger)
    {
        _studentDomainService = studentDomainService;
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindByIdAsync(request.Id);
        if (student == null)
        {
            throw new StudentNotFoundException(request.Id);
        }

        await _studentDomainService.UpdateContactInfoAsync(
            student, 
            request.FullName, 
            request.Gender, 
            request.Birth,
            request.ImageUrl, 
            request.CitizenId, 
            request.Email, 
            request.Phone, 
            request.HomeTown, 
            request.Address);
        
        await _studentDomainService.UpdateEducationInfoAsync(
            student, 
            request.Code, 
            request.HomeRoomId, 
            request.EducationProgramId);
        
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Updated student information with id '{StudentId}'", student.Id);
    }
}