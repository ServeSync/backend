using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationContactHasAttendAnEventException : ResourceInvalidOperationException
{
    public EventOrganizationContactHasAttendAnEventException(Guid id) 
        : base($"Event organization contact with id '{id}' has attended an event", ErrorCodes.EventOrganizationContactHasAttendAnEvent)
    {
    }
}