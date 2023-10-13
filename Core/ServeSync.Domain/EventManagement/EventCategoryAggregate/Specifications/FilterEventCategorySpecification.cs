using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;

public class FilterEventCategorySpecification : PagingAndSortingSpecification<EventCategory, Guid>
{
    private readonly string? _search;

    public FilterEventCategorySpecification(int page, int size, string sorting, string? search) : base(page, size, sorting)
    {
        _search = search;
    }
    
    public override Expression<Func<EventCategory, bool>> ToExpression()
    {
        return x => string.IsNullOrWhiteSpace(_search) || x.Name.ToLower().Contains(_search.ToLower());
    }
}