using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;

public class EventActivityFromEventCategorySpecification : Specification<EventActivity, Guid>
{
    private readonly Guid _id;
    
    public EventActivityFromEventCategorySpecification(Guid id)
    {
        _id = id;
        
        AddInclude(x => x.EventCategory!);
    }
    
    public override Expression<Func<EventActivity, bool>> ToExpression()
    {
        return x => x.Id == _id && x.EventCategory!.Type == EventCategoryType.Event;
    }
}