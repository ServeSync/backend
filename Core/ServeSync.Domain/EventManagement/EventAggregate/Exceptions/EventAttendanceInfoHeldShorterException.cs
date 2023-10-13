using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventAttendanceInfoHeldShorterException : ResourceInvalidDataException
{
    public EventAttendanceInfoHeldShorterException() 
        : base($"Event attendance should be held at least 15 min!", ErrorCodes.EventAttendanceInfoHeldShorter)
    {
    }
}