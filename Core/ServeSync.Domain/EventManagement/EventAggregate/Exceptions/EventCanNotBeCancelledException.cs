using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventCanNotBeCancelledException : ResourceInvalidDataException
{
    public EventCanNotBeCancelledException() 
        : base("Event can not be cancelled!", ErrorCodes.EventCanNotBeCancelled)
    {
    }
}