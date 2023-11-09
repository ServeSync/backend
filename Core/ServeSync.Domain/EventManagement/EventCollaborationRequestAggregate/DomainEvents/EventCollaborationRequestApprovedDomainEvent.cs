using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainEvents;

public class EventCollaborationRequestApprovedDomainEvent : IDomainEvent
{
    public EventCollaborationRequest EventCollaborationRequest { get; set;  }
    public EventCollaborationRequestApprovedDomainEvent(EventCollaborationRequest eventCollaborationRequest)
    {
        EventCollaborationRequest = eventCollaborationRequest;
    }
}