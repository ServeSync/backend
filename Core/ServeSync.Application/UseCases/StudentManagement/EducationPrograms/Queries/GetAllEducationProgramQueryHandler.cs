using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Queries;

public class GetAllEducationProgramQueryHandler : IQueryHandler<GetAllEducationProgramQuery, IEnumerable<EducationProgramDto>>
{
    private readonly IBasicReadOnlyRepository<EducationProgram, Guid> _educationProgramRepository;
    private readonly IMapper _mapper;

    public GetAllEducationProgramQueryHandler(
        IBasicReadOnlyRepository<EducationProgram, Guid> educationProgramRepository,
        IMapper mapper)
    {
        _educationProgramRepository = educationProgramRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<EducationProgramDto>> Handle(GetAllEducationProgramQuery request, CancellationToken cancellationToken)
    {
        var educationPrograms = await _educationProgramRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<EducationProgramDto>>(educationPrograms);
    }
}