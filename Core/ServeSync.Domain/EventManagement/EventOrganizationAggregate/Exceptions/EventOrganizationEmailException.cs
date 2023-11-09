using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationEmailException : ResourceAlreadyExistException
{
    public EventOrganizationEmailException(string email) 
        : base(nameof(EventOrganization), nameof(EventOrganization.Email), email, ErrorCodes.DuplicateEventOrganizationEmail)
    {
    }
}