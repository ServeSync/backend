using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;

public class EventOrganizationInvitationApprovedDomainEvent : IDomainEvent
{
    public EventOrganization Organization { get; set; }
    
    public EventOrganizationInvitationApprovedDomainEvent(EventOrganization organization)
    {
        Organization = organization;
    }
}