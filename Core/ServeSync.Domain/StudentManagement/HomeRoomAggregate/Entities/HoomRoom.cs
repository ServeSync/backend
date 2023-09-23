using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

public class HomeRoom : AggregateRoot
{
    public string Name { get; private set; }
    
    public Guid FacultyId { get; private set; }
    public Faculty? Faculty { get; private set; }

    public HomeRoom(string name, Guid facultyId)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        FacultyId = Guard.NotNull(facultyId, nameof(FacultyId));
    }
    
    private HomeRoom()
    {
        
    }
}