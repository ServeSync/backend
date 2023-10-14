using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRegistrationOverlappedWithEventException : ResourceInvalidDataException
{
    public EventRegistrationOverlappedWithEventException() 
        : base("Event registration is overlapped with event!", ErrorCodes.EventRegistrationOverlappedWithEvent)
    {
    }
}