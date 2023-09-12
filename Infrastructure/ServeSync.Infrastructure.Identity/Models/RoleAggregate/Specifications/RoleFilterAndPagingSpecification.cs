using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Specifications;

public class RoleFilterAndPagingSpecification : PagingAndSortingSpecification<ApplicationRole, string>
{
    private readonly string _name;
    public RoleFilterAndPagingSpecification(int page, int size, string sorting, string name) 
        : base(page, size, sorting, false)
    {
        _name = name;
    }

    public override Expression<Func<ApplicationRole, bool>> ToExpression()
    {
        return x => x.Name.Contains(_name);
    }
}