using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.HomeRooms.Queries;

public class GetAllHomeRoomQuery : IQuery<IEnumerable<HomeRoomDto>>
{
    public Guid? FacultyId { get; set; }
    
    public GetAllHomeRoomQuery(Guid? facultyId)
    {
        FacultyId = facultyId;
    }
}