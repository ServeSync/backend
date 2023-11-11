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
    public List<OrganizationRepInEvent> Representatives { get; private set;}
    
    internal OrganizationInEvent(Guid organizationId, string role, Guid eventId)
    {
        OrganizationId = Guard.NotNull(organizationId, nameof(OrganizationId));
        EventId = Guard.NotNull(eventId, nameof(EventId));
        Role = Guard.NotNullOrWhiteSpace(role, nameof(Role));
        Representatives = new List<OrganizationRepInEvent>();
    }

    internal void Update(Guid organizationId, string role)
    {
        if (OrganizationId != organizationId)
        {
            OrganizationId = Guard.NotNull(organizationId, nameof(OrganizationId));
            Representatives.Clear();
        }
        
        Role = Guard.NotNullOrWhiteSpace(role, nameof(Role));
    }

    internal void AddRepresentative(Guid representativeId, string role)
    {
        if (Representatives.Any(x => x.OrganizationRepId == representativeId))
        {
            throw new RepresentativeHasAlreadyAddedToEventException(EventId, representativeId);
        }
        
        Representatives.Add(new OrganizationRepInEvent(Id, representativeId, role));
    }
    
    internal void UpdateRepresentative(Guid id, Guid representativeId, string role)
    {
        var representative = Representatives.FirstOrDefault(x => x.Id == id);
        if (representative == null)
        {
            throw new RepresentativeNotFoundInEventException(id, OrganizationId, EventId);
        }
        
        if (Representatives.Any(x => x.OrganizationRepId == representativeId && x.Id != id))
        {
            throw new RepresentativeHasAlreadyAddedToEventException(EventId, representative.OrganizationRepId);
        }
        
        representative.Update(representativeId, role);
    }
    
    internal void RemoveRepresentative(Guid id)
    {
        var representative = Representatives.FirstOrDefault(x => x.Id == id);
        if (representative == null)
        {
            throw new RepresentativeNotFoundInEventException(id, OrganizationId, EventId);
        }
        
        Representatives.Remove(representative);
    }
    
    private OrganizationInEvent()
    {
        Representatives = new List<OrganizationRepInEvent>();
    }
}