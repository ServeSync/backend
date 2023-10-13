using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventHeldShorterException : ResourceInvalidDataException
{
    public EventHeldShorterException() 
        : base("Event should be held at least 1h!", ErrorCodes.EventHeldShorter)
    {
    }
}