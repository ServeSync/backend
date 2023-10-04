using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.HomeRoomAggregate.DomainServices;

public interface IHomeRoomDomainService
{
    Task<HomeRoom> CreateAsync(string name, Guid facultyId);
}