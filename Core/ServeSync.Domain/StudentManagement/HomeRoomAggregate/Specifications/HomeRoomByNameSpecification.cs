using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.HomeRoomAggregate.Specifications;

public class HomeRoomByNameSpecification : Specification<HomeRoom, Guid>
{
    private readonly string _name;

    public HomeRoomByNameSpecification(string name)
    {
        _name = name;
    }
    
    public override Expression<Func<HomeRoom, bool>> ToExpression()
    {
        return x => x.Name == _name;
    }
}