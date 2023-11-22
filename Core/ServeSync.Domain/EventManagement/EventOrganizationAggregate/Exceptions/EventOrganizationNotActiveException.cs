using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationNotActiveException : ResourceInvalidDataException
{
    public EventOrganizationNotActiveException(Guid id) 
        : base($"Event organization with Id '{id}' does not in active status!", ErrorCodes.EventOrganizationNotActive)
    {
    }
}