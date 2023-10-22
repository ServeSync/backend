namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

public class EventRegisterDto
{
    public Guid EventRoleId { get; set; }
    public string? Description { get; set; }
}