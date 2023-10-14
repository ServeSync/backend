using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class EventByTypeSpecification : Specification<Event, Guid>
{
    private readonly EventType _type;
    
    public EventByTypeSpecification(EventType type)
    {
        _type = type;
    }
    
    public override Expression<Func<Event, bool>> ToExpression()
    {
        return x => x.Type == _type;
    }
}