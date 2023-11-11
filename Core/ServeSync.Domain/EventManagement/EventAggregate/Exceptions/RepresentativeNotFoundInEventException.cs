using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class RepresentativeNotFoundInEventException : ResourceNotFoundException
{
    public RepresentativeNotFoundInEventException(Guid representativeId, Guid organizationId, Guid eventId) 
        : base($"Representative in event '{representativeId}' of organization '{organizationId}' does not exist in event '{eventId}'!", ErrorCodes.RepresentativeNotFoundInEvent)
    {
    }
}