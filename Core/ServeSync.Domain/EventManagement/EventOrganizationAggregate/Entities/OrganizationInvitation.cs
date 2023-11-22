using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

public class OrganizationInvitation : AggregateRoot
{
    public Guid ReferenceId { get; private set; }
    public InvitationType Type { get; private set; }
    public string Code { get; private set; }
    
    public OrganizationInvitation(Guid referenceId, InvitationType type, string code)
    {
        ReferenceId = referenceId;
        Type = type;
        Code = code;
    }
    
    private OrganizationInvitation()
    {
        
    }
}