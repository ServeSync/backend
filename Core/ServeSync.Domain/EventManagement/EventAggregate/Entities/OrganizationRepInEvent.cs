using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventAggregate.Entities;

public class OrganizationRepInEvent : Entity
{
    public Guid OrganizationInEventId { get; private set; }
    public OrganizationInEvent? OrganizationInEvent { get; private set; }
    
    public Guid OrganizationRepId { get; private set; }
    public EventOrganizationContact? OrganizationRep { get; private set; }
    
    public string Role { get; private set; }
    
    public OrganizationRepInEvent(Guid organizationInEventId, Guid organizationRepId, string role)
    {
        OrganizationInEventId = Guard.NotNull(organizationInEventId, nameof(OrganizationInEventId));
        OrganizationRepId = Guard.NotNull(organizationRepId, nameof(OrganizationRepId));
        Role = Guard.NotNullOrWhiteSpace(role, nameof(Role));
    }

    private OrganizationRepInEvent()
    {
        
    }
}