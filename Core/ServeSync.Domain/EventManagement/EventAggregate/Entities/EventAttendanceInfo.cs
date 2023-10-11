using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventAggregate.Entities;

public class EventAttendanceInfo : Entity
{
    public string Code { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    
    public Guid EventId { get; private set; }
    public Event? Event { get; private set; }

    public EventAttendanceInfo(string code, DateTime startAt, DateTime endAt, Guid eventId)
    {
        Code = Guard.NotNullOrWhiteSpace(code, nameof(Code));
        StartAt = Guard.NotNull(startAt, nameof(StartAt));
        EndAt = Guard.Range(endAt, nameof(EndAt), startAt);
        EventId = Guard.NotNull(eventId, nameof(EventId));
    }

    private EventAttendanceInfo()
    {
        
    }
}