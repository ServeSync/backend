using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventCanNotBeApprovedException : ResourceInvalidDataException
{
    public EventCanNotBeApprovedException()
        :base("Event can not be approved!", ErrorCodes.EventCanNotBeApproved)
    {
    }
}