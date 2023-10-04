using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Faculties.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Faculties.Queries;

public class GetAllFacultyQueryHandler : IQueryHandler<GetAllFacultyQuery, IEnumerable<FacultyDto>>
{
    private readonly IFacultyCachingManager _facultyCachingManager;

    public GetAllFacultyQueryHandler(IFacultyCachingManager facultyCachingManager)
    {
        _facultyCachingManager = facultyCachingManager;
    }
    
    public async Task<IEnumerable<FacultyDto>> Handle(GetAllFacultyQuery request, CancellationToken cancellationToken)
    {
        return await _facultyCachingManager.GetOrAddAsync();
    }
}