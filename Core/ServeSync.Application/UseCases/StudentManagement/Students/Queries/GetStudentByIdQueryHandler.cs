using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Queries;

public class GetStudentByIdQueryHandler : IQueryHandler<GetStudentByIdQuery, FlatStudentDto>
{
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IMapper _mapper;
    
    public GetStudentByIdQueryHandler(IBasicReadOnlyRepository<Student, Guid> studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }
    
    public async Task<FlatStudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindByIdAsync(request.Id);
        if (student == null)
        {
            throw new StudentNotFoundException(request.Id);
        }
        
        return _mapper.Map<FlatStudentDto>(student);
    }
}