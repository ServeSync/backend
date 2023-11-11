namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;

public class EventAttendanceInfoDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? QrCodeUrl { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }

    public AttendanceInfoStatus Status
    {
        get
        {
            var now = DateTime.UtcNow;
            if (now < StartAt)
            {
                return AttendanceInfoStatus.Upcoming;
            }
            else if (now > EndAt)
            {
                return AttendanceInfoStatus.Done;
            }
            else
            {
                return AttendanceInfoStatus.Happening;
            }
        }
    }
}

public enum AttendanceInfoStatus
{
    Done,
    Happening,
    Upcoming
}