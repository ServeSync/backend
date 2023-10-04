using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.HomeRoomAggregate.Specifications;

public class HomeRoomsByIncludedNamesSpecification : Specification<HomeRoom, Guid>
{
    private readonly IEnumerable<string> _names;
    
    public HomeRoomsByIncludedNamesSpecification(IEnumerable<string> names)
    {
        _names = names;
    }
    
    public override Expression<Func<HomeRoom, bool>> ToExpression()
    {
        return x => _names.Contains(x.Name);
    }
}