using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class NotInEventAttendanceTimeException : ResourceInvalidDataException
{
    public NotInEventAttendanceTimeException(Guid eventId) 
        : base($"Not in attendance time for event '{eventId}'", ErrorCodes.NotInEventAttendanceTime)
    {
    }
}