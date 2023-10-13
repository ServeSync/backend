using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class EventOrganizationContactDoesNotBelongToOrganizationException : ResourceInvalidDataException
{
    public EventOrganizationContactDoesNotBelongToOrganizationException(Guid organizationId, Guid organizationContactId) 
        : base($"Contact with Id '{organizationContactId}' does not belong to organization '{organizationId}'",ErrorCodes.EventOrganizationContactDoesNotBelongToOrganization)
    {
    }
}