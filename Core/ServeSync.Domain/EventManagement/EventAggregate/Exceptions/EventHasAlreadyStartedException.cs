using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventHasAlreadyStartedException : ResourceInvalidOperationException
{
    public EventHasAlreadyStartedException(Guid id) 
        : base($"Event with Id '{id}' has already started!", ErrorCodes.EventHasAlreadyStarted)
    {
    }
}