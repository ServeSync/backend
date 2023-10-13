using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventAttendanceInfoOverlappedException : ResourceAlreadyExistException
{
    public EventAttendanceInfoOverlappedException(DateTime startAt, DateTime endAt) 
        : base($"Event attendance from '{startAt}' - '{endAt}' has been overlapped!", ErrorCodes.EventAttendanceInfoOverlapped)
    {
    }
}