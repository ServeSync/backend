using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class OrganizationHasNotAddedToEventException : ResourceNotFoundException
{
    public OrganizationHasNotAddedToEventException(Guid eventId, Guid organizationId) 
        : base($"Organization with Id '{organizationId}' has not added to event with Id '{eventId}'!", ErrorCodes.OrganizationHasNotAddedToEvent)
    {
    }
}