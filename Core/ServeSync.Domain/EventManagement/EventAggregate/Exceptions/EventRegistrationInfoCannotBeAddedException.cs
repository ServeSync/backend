using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRegistrationInfoCannotBeAddedException : ResourceInvalidDataException
{
    public EventRegistrationInfoCannotBeAddedException() 
        : base("Event registration can not be added!", ErrorCodes.EventRegistrationInfoCannotBeAdded)
    {
    }
}