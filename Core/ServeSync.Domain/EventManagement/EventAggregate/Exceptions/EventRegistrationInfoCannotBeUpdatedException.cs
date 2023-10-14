using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRegistrationInfoCannotBeUpdatedException : ResourceInvalidOperationException
{
    public EventRegistrationInfoCannotBeUpdatedException(Guid id) 
        : base($"Can not update registration info of event '{id}'!", ErrorCodes.EventRegistrationInfoCannotBeUpdated)
    {
    }
}