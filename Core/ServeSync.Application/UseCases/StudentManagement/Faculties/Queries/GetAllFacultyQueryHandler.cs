using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Faculties.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;

namespace ServeSync.Application.UseCases.StudentManagement.Faculties.Queries;

public class GetAllFacultyQueryHandler : IQueryHandler<GetAllFacultyQuery, IEnumerable<FacultyDto>>
{
    private readonly IBasicReadOnlyRepository<Faculty, Guid> _facultyRepository;
    private readonly IMapper _mapper;

    public GetAllFacultyQueryHandler(
        IBasicReadOnlyRepository<Faculty, Guid> facultyRepository,
        IMapper mapper)
    {
        _facultyRepository = facultyRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<FacultyDto>> Handle(GetAllFacultyQuery request, CancellationToken cancellationToken)
    {
        var faculties = await _facultyRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<FacultyDto>>(faculties);
    }
}