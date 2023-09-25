using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class DeleteStudentCommandHandler : ICommandHandler<DeleteStudentCommand>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentDomainService _studentDomainService;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteStudentCommandHandler(
        IStudentRepository studentRepository,
        IStudentDomainService studentDomainService, 
        IUnitOfWork unitOfWork)
    {
        _studentRepository = studentRepository;
        _studentDomainService = studentDomainService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindByIdAsync(request.Id);
        if (student == null)
        {
            throw new StudentNotFoundException(request.Id);
        }
        
        _studentDomainService.Delete(student);
        await _unitOfWork.CommitAsync();
    }
}