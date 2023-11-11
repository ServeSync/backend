using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventAttendanceInfoCanNotBeUpdatedException : ResourceInvalidDataException
{
    public EventAttendanceInfoCanNotBeUpdatedException(Guid id) 
        : base($"Event attendance info with for event with id '{id}' can not be updated!", ErrorCodes.EventAttendanceInfoCanNotBeUpdated)
    {
    }
}