using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Specifications;

public class UserWithRoleSpecification : Specification<ApplicationUser, string>
{
    private readonly string _userId;
    
    public UserWithRoleSpecification(string userId)
    {
        _userId = userId;
    }
    
    public override Expression<Func<ApplicationUser, bool>> ToExpression()
    {
        return x => x.Id == _userId;
    }
}