namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;

public class EventAttendanceInfoDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string QrCodeUrl { get; set; } = null!;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}