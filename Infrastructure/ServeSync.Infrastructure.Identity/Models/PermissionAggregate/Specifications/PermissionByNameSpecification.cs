using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Specifications;

public class PermissionByNameSpecification : Specification<ApplicationPermission, Guid>
{
    private readonly string _name;
    
    public PermissionByNameSpecification(string name)
    {
        _name = name;
    }

    public override Expression<Func<ApplicationPermission, bool>> ToExpression()
    {
        return x => x.Name.Contains(_name);
    }
}