using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationContactNotPendingException : ResourceInvalidDataException
{
    public EventOrganizationContactNotPendingException(Guid id) 
        : base($"Event organization contact with Id '{id}' does not in pending status!", ErrorCodes.EventOrganizationContactNotPending)
    {
    }
}