namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRegistrationInfos;

public class EventRegistrationDto
{
    public Guid Id { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public RegistrationInfoStatus Status
    {
        get
        {
            var now = DateTime.UtcNow;
            if (now < StartAt)
            {
                return RegistrationInfoStatus.Upcoming;
            }
            else if (now > EndAt)
            {
                return RegistrationInfoStatus.Done;
            }
            else
            {
                return RegistrationInfoStatus.Happening;
            }
        }
    }
}

public enum RegistrationInfoStatus
{
    Done,
    Happening,
    Upcoming
}