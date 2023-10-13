using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class OrganizationHasAlreadyAddedToEventException : ResourceAlreadyExistException
{
    public OrganizationHasAlreadyAddedToEventException(Guid organizationId, Guid eventId) 
        : base($"Organization with Id '{organizationId}' has already added to event '{eventId}'!", ErrorCodes.OrganizationHasAlreadyAddedToEvent)
    {
    }
}