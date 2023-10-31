using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Specifications;

public class FilterEventCollaborationRequestSpecification : PagingAndSortingSpecification<EventCollaborationRequest, Guid>
{
    private readonly string? _search;
    
    public FilterEventCollaborationRequestSpecification(int page, int size, string sorting, string? search) : base(page, size, sorting)
    {
        _search = search;
        
        AddInclude(x => x.Activity!);
    }
    
    public override Expression<Func<EventCollaborationRequest, bool>> ToExpression()
    {
        return x => string.IsNullOrWhiteSpace(_search) ||
                    x.Name.ToLower().Contains(_search.ToLower()) ||
                    x.Address.FullAddress.ToLower().Contains(_search.ToLower()) ||
                    x.Organization!.Name.ToLower().Contains(_search.ToLower());
    }
}