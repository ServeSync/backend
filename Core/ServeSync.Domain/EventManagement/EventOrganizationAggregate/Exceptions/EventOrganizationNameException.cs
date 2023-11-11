using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationNameException : ResourceAlreadyExistException
{
    public EventOrganizationNameException(string name)
        : base(nameof(EventOrganization), nameof(EventOrganization.Name), name,
            ErrorCodes.DuplicateEventOrganizationName)
    {
    }
}
