namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;

public class EventAttendanceInfoUpdateDto
{
    public Guid? Id { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}