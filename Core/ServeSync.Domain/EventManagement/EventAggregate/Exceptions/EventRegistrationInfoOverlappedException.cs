using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRegistrationInfoOverlappedException : ResourceInvalidDataException
{
    public EventRegistrationInfoOverlappedException() 
        : base($"Event registration from has been overlapped!", ErrorCodes.EventRegistrationInfoOverlapped)
    {
    }
}