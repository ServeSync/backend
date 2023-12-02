using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;

public class EventActivityByTypeSpecification : Specification<EventActivity, Guid>
{
    private readonly EventCategoryType _type;
    
    public EventActivityByTypeSpecification(EventCategoryType type)
    {
        _type = type;
        
        AddInclude(x => x.EventCategory!);
    }
    
    public override Expression<Func<EventActivity, bool>> ToExpression()
    {
        return x => x.EventCategory!.Type == _type;
    }
}