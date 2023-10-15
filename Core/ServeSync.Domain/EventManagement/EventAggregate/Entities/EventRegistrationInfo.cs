using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventAggregate.Entities;

public class EventRegistrationInfo : Entity
{
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    
    public Guid EventId { get; private set; }
    public Event? Event { get; private set; }
    
    internal EventRegistrationInfo(DateTime startAt, DateTime endAt, Guid eventId)
    {
        StartAt = Guard.Range(startAt, nameof(StartAt), DateTime.Now);
        SetEndAt(endAt);
        EventId = Guard.NotNull(eventId, nameof(EventId));
    }
    
    public void SetEndAt(DateTime endAt)
    {
        if (endAt < StartAt.AddMinutes(15))
        {
            throw new EventRegistrationHeldShorterException();
        }
        EndAt = Guard.Range(endAt, nameof(EndAt), StartAt.AddMinutes(15));
        EndAt = endAt;
    }
    
    public bool IsOverlapped(DateTime startAt, DateTime endAt)
    {
        return (StartAt <= endAt && EndAt >= endAt) || (StartAt <= startAt && EndAt >= startAt);
    }
    
    private EventRegistrationInfo()
    {
    }
}