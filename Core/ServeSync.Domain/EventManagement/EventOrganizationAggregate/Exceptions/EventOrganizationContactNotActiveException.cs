using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationContactNotActiveException : ResourceInvalidDataException
{
    public EventOrganizationContactNotActiveException(Guid id) 
        : base($"Event organization contact with Id '{id}' does not in active status!", ErrorCodes.EventOrganizationContactNotActive)
    {
    }
}