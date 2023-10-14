using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRoleIsFullRegisteredException : ResourceInvalidOperationException
{
    public EventRoleIsFullRegisteredException(Guid id) 
        : base($"Event role with id '{id}' is full of registered!", ErrorCodes.EventRoleIsFullRegistered)
    {
    }
}