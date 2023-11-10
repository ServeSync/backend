using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationHasHostedAnEventException : ResourceInvalidOperationException
{
    public EventOrganizationHasHostedAnEventException(Guid id) 
        : base($"Event organization with id '{id}' has host an event", ErrorCodes.EventOrganizationHasHostedEvent)
    {
    }
}