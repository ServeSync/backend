using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class StudentByHomeRoomSpecification : Specification<Student, Guid>
{
    private readonly Guid _homeRoomId;
    
    public StudentByHomeRoomSpecification(Guid homeRoomId)
    {
        _homeRoomId = homeRoomId;
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => x.HomeRoomId == _homeRoomId;
    }
}