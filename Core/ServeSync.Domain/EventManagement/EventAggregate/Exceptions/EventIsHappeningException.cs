using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventIsHappeningException : ResourceInvalidDataException
{
    public EventIsHappeningException(Guid id) 
        : base($"Event with Id '{id}' is happening!", ErrorCodes.EventIsHappening)
    {
    }
}