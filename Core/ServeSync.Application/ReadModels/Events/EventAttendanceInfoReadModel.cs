using ServeSync.Application.ReadModels.Abstracts;

namespace ServeSync.Application.ReadModels.Events;

public class EventAttendanceInfoReadModel : BaseReadModel<Guid>
{
    public string? Code { get; set; }
    public string? QrCodeUrl { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    
    public List<AttendanceStudentInEventRoleReadModel> AttendanceStudents { get; set; } = new();
}

public class AttendanceStudentInEventRoleReadModel : BaseReadModel<Guid>
{
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public DateTime AttendanceAt { get; set; }
    public string IdentityId { get; set; } = null!;
}