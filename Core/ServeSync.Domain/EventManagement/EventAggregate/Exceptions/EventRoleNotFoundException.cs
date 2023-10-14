using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventRoleNotFoundException : ResourceNotFoundException
{
    public EventRoleNotFoundException(Guid id) : base(nameof(EventRole), id, ErrorCodes.EventRoleNotFound)
    {
    }
}