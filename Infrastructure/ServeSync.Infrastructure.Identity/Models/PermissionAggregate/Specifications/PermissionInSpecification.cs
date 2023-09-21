using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Specifications;

public class PermissionInSpecification : Specification<ApplicationPermission, Guid>
{
    private readonly IEnumerable<Guid> _permissionsIds;
    
    public PermissionInSpecification(IEnumerable<Guid> permissionsIds)
    {
        _permissionsIds = permissionsIds;
    }
    
    public override Expression<Func<ApplicationPermission, bool>> ToExpression()
    {
        return x => _permissionsIds.Contains(x.Id);
    }
}