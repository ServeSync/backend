using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;

public class EventActivityNotFoundException : ResourceNotFoundException
{
    public EventActivityNotFoundException(Guid id) 
        : base(nameof(EventActivity), id, ErrorCodes.EventActivityNotFound)
    {
    }
}