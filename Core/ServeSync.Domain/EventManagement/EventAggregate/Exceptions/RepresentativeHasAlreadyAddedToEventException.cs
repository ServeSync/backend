using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class RepresentativeHasAlreadyAddedToEventException : ResourceAlreadyExistException
{
    public RepresentativeHasAlreadyAddedToEventException(Guid eventId, Guid representativeId) 
        : base($"Representative with Id '{representativeId}' has already been added to event with Id '{eventId}'", ErrorCodes.RepresentativeHasAlreadyAddedToEvent)
    {
    }
}