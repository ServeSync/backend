﻿using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainEvents;

public class EventCollaborationRequestRejectedDomainEvent : IDomainEvent
{
    public EventCollaborationRequest EventCollaborationRequest { get; set;  }
    public EventCollaborationRequestRejectedDomainEvent(EventCollaborationRequest eventCollaborationRequest)
    {
        EventCollaborationRequest = eventCollaborationRequest;
    }
}