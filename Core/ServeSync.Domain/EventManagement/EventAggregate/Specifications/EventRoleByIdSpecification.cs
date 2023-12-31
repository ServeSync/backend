﻿using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class EventRoleByIdSpecification : Specification<EventRole, Guid>
{
    private readonly Guid _eventRoleId;
    
    public EventRoleByIdSpecification(Guid eventRoleId) : base(true)
    {
        _eventRoleId = eventRoleId;
        AddInclude(x => x.Event!);
        AddInclude("Event.RegistrationInfos");
        AddInclude("Event.AttendanceInfos");
        AddInclude("Event.Roles");
    }
    
    public override Expression<Func<EventRole, bool>> ToExpression()
    {
        return x => x.Id == _eventRoleId;
    }
}