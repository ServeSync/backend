using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class FilterEventOrganizationContactSpecification : PagingAndSortingSpecification<EventOrganizationContact, Guid>
{
    private readonly string? _search;

    public FilterEventOrganizationContactSpecification(int page, int size, string sorting, string? search) : base(page, size, sorting)
    {
        _search = search;
    }
    
    public override Expression<Func<EventOrganizationContact, bool>> ToExpression()
    {
        return x => string.IsNullOrWhiteSpace(_search) || x.Name.ToLower().Contains(_search.ToLower());
    }
}