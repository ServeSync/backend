using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventOrganizationCanNotBeUpdatedException : ResourceInvalidOperationException
{
    public EventOrganizationCanNotBeUpdatedException(Guid id) 
        : base($"Event organization for event with Id '{id}' can not be updated due to event has already started!", ErrorCodes.EventOrganizationCanNotBeUpdated)
    {
    }
}