using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class UpdateStudentByIdentityCommandHandler : ICommandHandler<UpdateStudentByIdentityCommand>
{
    private readonly IStudentDomainService _studentDomainService;
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<UpdateStudentByIdentityCommandHandler> _logger;
    
    public UpdateStudentByIdentityCommandHandler(
        IStudentDomainService studentDomainService, 
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        ILogger<UpdateStudentByIdentityCommandHandler> logger)
    {
        _studentDomainService = studentDomainService;
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _logger = logger;
    }

    
    public async Task Handle(UpdateStudentByIdentityCommand request, CancellationToken cancellationToken)
    {
        if (!await _currentUser.IsStudentAsync())
        {
            throw new ResourceAccessDeniedException("Only student can access this resource.");
        }
        
        var student = await _studentRepository.FindAsync(new StudentByIdentitySpecification(_currentUser.Id));
        if (student == null)
        {
            throw new StudentNotFoundException(_currentUser.Id);
        }
        
        await _studentDomainService.UpdateContactInfoAsync(
            student, 
            student.FullName, 
            student.Gender, 
            student.DateOfBirth,
            request.ImageUrl, 
            student.CitizenId, 
            request.Email, 
            request.Phone, 
            request.HomeTown, 
            request.Address);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Updated student information with id '{StudentId}'", student.Id);
    }
}