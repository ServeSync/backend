using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationNotPendingException : ResourceInvalidDataException
{
    public EventOrganizationNotPendingException(Guid id) 
        : base($"Event organization with Id '{id}' does not in pending status!", ErrorCodes.EventOrganizationNotPending)
    {
    }
}