using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;

public class EventCategoryByTypeSpecification : Specification<EventCategory, Guid>
{
    private readonly EventCategoryType _type;
    
    public EventCategoryByTypeSpecification(EventCategoryType type)
    {
        _type = type;
    }
    
    public override Expression<Func<EventCategory, bool>> ToExpression()
    {
        return x => x.Type == _type;
    }
}