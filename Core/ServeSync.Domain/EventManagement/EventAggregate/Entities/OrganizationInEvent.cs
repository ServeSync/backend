using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventAggregate.Entities;

public class OrganizationInEvent : Entity
{
    public Guid OrganizationId { get; private set; }
    public EventOrganization? Organization { get; private set; }
    
    public Guid EventId { get; private set; }
    public Event? Event { get; private set; }
    
    public string Role { get; private set; }
    public List<OrganizationRepInEvent> Representatives {get; private set;}
    
    internal OrganizationInEvent(Guid organizationId, string role, Guid eventId)
    {
        OrganizationId = Guard.NotNull(organizationId, nameof(OrganizationId));
        EventId = Guard.NotNull(eventId, nameof(EventId));
        Role = Guard.NotNullOrWhiteSpace(role, nameof(Role));
        Representatives = new List<OrganizationRepInEvent>();
    }

    internal void AddRepresentative(Guid representativeId, string role)
    {
        if (Representatives.Any(x => x.OrganizationRepId == representativeId))
        {
            throw new RepresentativeHasAlreadyAddedToEventException(EventId, representativeId);
        }
        
        Representatives.Add(new OrganizationRepInEvent(Id, representativeId, role));
    }
    
    private OrganizationInEvent()
    {
        Representatives = new List<OrganizationRepInEvent>();
    }
}