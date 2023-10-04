using ServeSync.Application.UseCases.StudentManagement.Faculties.Dtos;

namespace ServeSync.Application.Caching.Interfaces;

public interface IFacultyCachingManager
{
    Task SetAsync(IList<FacultyDto> faculties);
    
    Task<IList<FacultyDto>?> GetAsync();

    Task<IList<FacultyDto>> GetOrAddAsync();
}