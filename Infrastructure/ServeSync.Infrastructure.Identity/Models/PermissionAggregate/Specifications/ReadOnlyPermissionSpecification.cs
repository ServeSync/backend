using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Specifications;

public class ReadOnlyPermissionSpecification : Specification<ApplicationPermission, Guid>
{
    public override Expression<Func<ApplicationPermission, bool>> ToExpression()
    {
        return x => x.Name.Contains("View");
    }
}