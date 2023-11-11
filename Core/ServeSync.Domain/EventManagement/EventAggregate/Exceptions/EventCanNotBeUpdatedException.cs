using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventCanNotBeUpdatedException : ResourceInvalidDataException
{
    public EventCanNotBeUpdatedException(Guid id) 
        : base($"Event with Id '{id}' can not be updated!", ErrorCodes.EventCanNotBeUpdated)
    {
    }
}