namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRegistrationInfos;

public class EventRegistrationUpdateDto
{
    public Guid? Id { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}