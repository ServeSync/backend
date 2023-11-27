using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class FilterEventOrganizationSpecification : PagingAndSortingSpecification<EventOrganization, Guid>
{
    private readonly string? _search;

    public FilterEventOrganizationSpecification(int page, int size, string sorting, string? search) : base(page, size, sorting)
    {
        _search = search;
        
        AddInclude(x => x.OrganizationInEvents);
    }
    
    public override Expression<Func<EventOrganization, bool>> ToExpression()
    {
        return x => string.IsNullOrWhiteSpace(_search) || 
                                    x.Name.ToLower().Contains(_search.ToLower()) || 
                                    x.Email.ToLower().Contains(_search.ToLower()) || 
                                    x.PhoneNumber.Contains(_search);
    }
}