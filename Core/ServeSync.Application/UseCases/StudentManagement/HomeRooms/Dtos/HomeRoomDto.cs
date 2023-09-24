namespace ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;

public class HomeRoomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid FacultyId { get; set; }
}