using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventCanNotBeRejectedException : ResourceInvalidDataException
{
    public EventCanNotBeRejectedException(Guid id) 
        : base($"Event with id {id} can not be rejected!", ErrorCodes.EventCanNotBeRejected)
    {
    }
}