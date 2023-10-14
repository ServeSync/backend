namespace ServeSync.API.Dtos.Events;

public class EventRegisterDto
{
    public Guid EventRoleId { get; set; }
    public string? Description { get; set; }
}