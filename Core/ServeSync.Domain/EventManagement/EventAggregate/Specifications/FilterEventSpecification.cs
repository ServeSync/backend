using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class FilterEventSpecification : PagingAndSortingSpecification<Event, Guid>
{
    private readonly string? _search;
    
    public FilterEventSpecification(int page, int size, string sorting, string? search) : base(page, size, sorting)
    {
        _search = search;
        
        AddInclude(x => x.Roles);
        AddInclude(x => x.RepresentativeOrganization!);
        AddInclude("RepresentativeOrganization.Organization");
    }
    
    public override Expression<Func<Event, bool>> ToExpression()
    {
        return x => string.IsNullOrWhiteSpace(_search) ||
                         x.Name.ToLower().Contains(_search.ToLower()) ||
                         x.Address.FullAddress.ToLower().Contains(_search.ToLower()) ||
                         x.RepresentativeOrganization!.Organization!.Name.ToLower().Contains(_search.ToLower());
    }

    public override string BuildSorting()
    {
        // Todo: filter by status
        
        Sorting = Sorting.Replace("RepresentativeOrganization", "RepresentativeOrganization.Organization.Name");
        Sorting = Sorting.Replace(nameof(Event.Address), "Address.FullAddress");
        return base.BuildSorting();
    }
}