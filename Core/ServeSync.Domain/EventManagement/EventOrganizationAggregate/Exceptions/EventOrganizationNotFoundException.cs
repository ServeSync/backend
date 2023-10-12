using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationNotFoundException : ResourceNotFoundException
{
    public EventOrganizationNotFoundException(Guid id) 
        : base(nameof(EventOrganization), id, ErrorCodes.EventOrganizationNotFound)
    {
    }
}