using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Queries;

public class GetAllStudentQueryHandler : IQueryHandler<GetAllStudentQuery, PagedResultDto<StudentDetailDto>>
{
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IMapper _mapper;

    public GetAllStudentQueryHandler(IBasicReadOnlyRepository<Student, Guid> studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<StudentDetailDto>> Handle(GetAllStudentQuery request, CancellationToken cancellationToken)
    {
        var specification = GetQuerySpecification(request);

        var students = await _studentRepository.GetPagedListAsync(specification, new StudentDetailDto());
        var total = await _studentRepository.GetCountAsync(specification);

        return new PagedResultDto<StudentDetailDto>(
            total, 
            request.Size,
            students);
    }

    private IPagingAndSortingSpecification<Student, Guid> GetQuerySpecification(GetAllStudentQuery request)
    {
        var specification = new FilterStudentSpecification(request.Page, request.Size, request.Sorting, request.Search, request.Gender)
            .AndIf(request.HomeRoomId.HasValue, new StudentByHomeRoomSpecification(request.HomeRoomId.GetValueOrDefault()))
            .AndIf(request.FacultyId.HasValue, new StudentByFacultySpecification(request.FacultyId.GetValueOrDefault()))
            .AndIf(request.EducationProgramId.HasValue, new StudentByEducationProgramSpecification(request.EducationProgramId.GetValueOrDefault()));

        return specification;
    }
}