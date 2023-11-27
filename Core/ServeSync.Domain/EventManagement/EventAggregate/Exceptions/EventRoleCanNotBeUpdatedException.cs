using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRoleCanNotBeUpdatedException : ResourceInvalidDataException
{
    public EventRoleCanNotBeUpdatedException(Guid id) 
        : base($"Event role for event with id '{id}' can not be updated!", ErrorCodes.EventRoleCanNotBeUpdated)
    {
    }
}