using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRoleIsFullApprovedException : ResourceInvalidOperationException
{
    public EventRoleIsFullApprovedException(Guid id) 
        : base($"Event role with id '{id}' is out of available position!", ErrorCodes.EventRoleIsFullRegistered)
    {
    }
}