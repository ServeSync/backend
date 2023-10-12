using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;

public class EventActivityByCategorySpecification : Specification<EventActivity, Guid>
{
    private readonly Guid _categoryId;

    public EventActivityByCategorySpecification(Guid categoryId)
    {
        _categoryId = categoryId;
    }
    
    public override Expression<Func<EventActivity, bool>> ToExpression()
    {
        return x => x.EventCategoryId == _categoryId;
    }
}