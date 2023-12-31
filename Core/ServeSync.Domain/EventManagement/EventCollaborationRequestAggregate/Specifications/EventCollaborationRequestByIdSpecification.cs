﻿using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Specifications;

public class EventCollaborationRequestByIdSpecification : Specification<EventCollaborationRequest, Guid>
{
    private readonly Guid _id;
    
    public EventCollaborationRequestByIdSpecification(Guid id)
    {
        _id = id;
        
        AddInclude(x => x.Activity!);
        AddInclude("Activity.EventCategory");
    }
    
    public override Expression<Func<EventCollaborationRequest, bool>> ToExpression()
    {
        return x => x.Id == _id;
    }
}