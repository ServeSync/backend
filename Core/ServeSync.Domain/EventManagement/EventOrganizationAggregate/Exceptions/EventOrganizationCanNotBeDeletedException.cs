using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationCanNotBeDeletedException : ResourceInvalidOperationException
{
    public EventOrganizationCanNotBeDeletedException(Guid id) 
        : base($"Event organization with id '{id}' can not be deleted", ErrorCodes.EventOrganizationCanNotBeDeleted)
    {
    }
}