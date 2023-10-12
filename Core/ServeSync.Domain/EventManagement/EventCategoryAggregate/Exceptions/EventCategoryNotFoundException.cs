using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;

public class EventCategoryNotFoundException : ResourceNotFoundException
{
    public EventCategoryNotFoundException(Guid id) 
        : base(nameof(EventCategory), id, ErrorCodes.EventCategoryNotFound)
    {
    }
}