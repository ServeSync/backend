using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationContactNotFoundException : ResourceNotFoundException
{
    public EventOrganizationContactNotFoundException(Guid id) 
        : base(nameof(EventOrganizationContact), id, ErrorCodes.EventOrganizationContactNotFound)
    {
    }
}