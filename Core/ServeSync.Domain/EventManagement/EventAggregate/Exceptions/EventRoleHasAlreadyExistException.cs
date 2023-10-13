using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRoleHasAlreadyExistException : ResourceAlreadyExistException
{
    public EventRoleHasAlreadyExistException(string name) 
        : base(nameof(EventRole), nameof(EventRole.Name), name, ErrorCodes.EventRoleHasAlreadyExist)
    {
    }
}