using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class EventByRoleSpecification : Specification<Event, Guid>
{
    private readonly Guid _eventRoleId;

    public EventByRoleSpecification(Guid eventRoleId)
    {
        _eventRoleId = eventRoleId;
        AddInclude(x => x.Roles);
    }    
    
    public override Expression<Func<Event, bool>> ToExpression()
    {
        return x => x.Roles.Any(y => y.Id == _eventRoleId);
    }
}