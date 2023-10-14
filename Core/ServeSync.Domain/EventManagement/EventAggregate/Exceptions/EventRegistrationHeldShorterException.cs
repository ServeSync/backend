using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRegistrationHeldShorterException : ResourceInvalidDataException
{
    public EventRegistrationHeldShorterException() 
        : base("Event registration should be held at least 15 min!", ErrorCodes.EventRegistrationHeldShorter)
    {
    }
}