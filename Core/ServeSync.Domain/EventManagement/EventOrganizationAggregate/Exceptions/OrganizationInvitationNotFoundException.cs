using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

public class OrganizationInvitationNotFoundException : ResourceNotFoundException
{
    public OrganizationInvitationNotFoundException(string code) 
        : base($"Organization invitation with Code '{code}' does not exist!", ErrorCodes.OrganizationInvitationNotFound)
    {
    }
}