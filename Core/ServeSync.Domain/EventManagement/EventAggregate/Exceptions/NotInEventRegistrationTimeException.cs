using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
 
public class NotInEventRegistrationTimeException : ResourceInvalidDataException
{
    public NotInEventRegistrationTimeException(Guid eventId) 
        : base($"Not in registration time for event '{eventId}'", ErrorCodes.NotInEventRegistrationTime)
    {
    }
}