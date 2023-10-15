using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventNotFoundException : ResourceNotFoundException
{
    public EventNotFoundException(Guid id) : base(nameof(Event), id, ErrorCodes.EventNotFound)
    {
    }
}