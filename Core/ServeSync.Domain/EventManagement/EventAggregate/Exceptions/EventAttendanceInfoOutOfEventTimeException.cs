using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventAttendanceInfoOutOfEventTimeException : ResourceInvalidDataException
{
    public EventAttendanceInfoOutOfEventTimeException() 
        : base("Event attendance info is out of event time!", ErrorCodes.EventAttendanceInfoOutOfEventTime)
    {
    }
}