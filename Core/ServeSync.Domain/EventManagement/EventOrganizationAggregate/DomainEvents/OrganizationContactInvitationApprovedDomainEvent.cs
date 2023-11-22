using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class OrganizationContactInvitationApprovedDomainEvent : IDomainEvent
{
    public EventOrganizationContact Contact { get; set; }
    public EventOrganization Organization { get; set; }
    
    public OrganizationContactInvitationApprovedDomainEvent(EventOrganizationContact contact, EventOrganization organization)
    {
        Contact = contact;
        Organization = organization;
    }
}