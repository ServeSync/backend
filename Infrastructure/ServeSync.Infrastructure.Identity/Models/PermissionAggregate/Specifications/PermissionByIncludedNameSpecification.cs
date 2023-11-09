using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Specifications;

public class PermissionByIncludedNameSpecification : Specification<ApplicationPermission, Guid>
{
    private readonly IEnumerable<string> _names;
    
    public PermissionByIncludedNameSpecification(IEnumerable<string> names)
    {
        _names = names;
    }

    public override Expression<Func<ApplicationPermission, bool>> ToExpression()
    {
        return x => _names.Contains(x.Name);
    }
}