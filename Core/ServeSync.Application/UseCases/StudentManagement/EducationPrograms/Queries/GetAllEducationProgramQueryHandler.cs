using AutoMapper;
using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Queries;

public class GetAllEducationProgramQueryHandler : IQueryHandler<GetAllEducationProgramQuery, IEnumerable<EducationProgramDto>>
{
    private readonly IEducationCachingManager _educationCachingManager;

    public GetAllEducationProgramQueryHandler(IEducationCachingManager educationCachingManager)
    {
        _educationCachingManager = educationCachingManager;
    }
    
    public async Task<IEnumerable<EducationProgramDto>> Handle(GetAllEducationProgramQuery request, CancellationToken cancellationToken)
    {
        return await _educationCachingManager.GetOrAddAsync();
    }
}