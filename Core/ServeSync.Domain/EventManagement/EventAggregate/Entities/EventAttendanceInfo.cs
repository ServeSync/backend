using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventAggregate.Entities;

public class EventAttendanceInfo : Entity
{
    public string Code { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    
    public Guid EventId { get; private set; }
    public Event? Event { get; private set; }

    internal EventAttendanceInfo(DateTime startAt, DateTime endAt, Guid eventId)
    {
        Code = Guid.NewGuid().ToString();
        StartAt = Guard.NotNull(startAt, nameof(StartAt));
        SetEndAt(endAt);
        EventId = Guard.NotNull(eventId, nameof(EventId));
    }
    
    public bool IsOverlapped(DateTime startAt, DateTime endAt)
    {
        return (StartAt <= endAt && EndAt >= endAt) || (StartAt <= startAt && EndAt >= startAt);
    }

    public void SetEndAt(DateTime endAt)
    {
        if (endAt < StartAt.AddMinutes(15))
        {
            throw new EventAttendanceInfoHeldShorterException();
        }
        EndAt = Guard.Range(endAt, nameof(EndAt), StartAt.AddMinutes(15));
    }

    private EventAttendanceInfo()
    {
        
    }
}