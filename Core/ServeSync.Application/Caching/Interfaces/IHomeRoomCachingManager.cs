using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;

namespace ServeSync.Application.Caching.Interfaces;

public interface IHomeRoomCachingManager
{
    Task SetAsync(IList<HomeRoomDto> homeRooms);
    
    Task<IList<HomeRoomDto>?> GetAsync();

    Task<IList<HomeRoomDto>> GetOrAddAsync();
    
    Task<IList<HomeRoomDto>> GetOrAddForFacultyAsync(Guid facultyId);
}