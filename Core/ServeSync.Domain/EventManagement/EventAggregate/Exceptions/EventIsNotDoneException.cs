using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventIsNotDoneException : ResourceInvalidDataException
{
    public EventIsNotDoneException(Guid id) 
        : base($"Event with Id '{id}' is not done yet!", ErrorCodes.EventIsNotDone)
    {
    }
}