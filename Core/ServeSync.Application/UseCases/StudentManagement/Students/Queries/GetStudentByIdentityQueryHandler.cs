using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Queries;

public class GetStudentByIdentityQueryHandler : IQueryHandler<GetStudentByIdentityQuery, FlatStudentDto>
{
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    
    public GetStudentByIdentityQueryHandler(
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IMapper mapper,
        ICurrentUser currentUser)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    
    public async Task<FlatStudentDto> Handle(GetStudentByIdentityQuery request, CancellationToken cancellationToken)
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
        
        return _mapper.Map<FlatStudentDto>(student);
    }
}