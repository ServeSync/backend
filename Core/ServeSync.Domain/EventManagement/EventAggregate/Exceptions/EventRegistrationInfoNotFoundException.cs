using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRegistrationInfoNotFoundException : ResourceNotFoundException
{
    public EventRegistrationInfoNotFoundException(Guid id, Guid eventId) 
        : base($"Event registration info with id '{id}' for event '{eventId}' does not exist!", ErrorCodes.EventRegistrationInfoNotFound)
    {
    }
}