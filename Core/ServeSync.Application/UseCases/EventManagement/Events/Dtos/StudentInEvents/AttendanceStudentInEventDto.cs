namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.StudentInEvents;

public class AttendanceStudentInEventDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string HomeRoomName { get; set; } = null!;
    public double Score { get; set; }
    public DateTime AttendanceAt { get; set; }
}