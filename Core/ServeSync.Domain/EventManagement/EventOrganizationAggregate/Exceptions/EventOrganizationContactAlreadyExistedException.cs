using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationContactAlreadyExistedException : ResourceAlreadyExistException
{
    public EventOrganizationContactAlreadyExistedException(Guid organizationId, string email) 
        : base($"Contact with email '{email}' has already added to organization with id '{organizationId}'", ErrorCodes.EventOrganizationContactAlreadyExisted)
    {
    }
}