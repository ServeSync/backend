using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.HomeRoomAggregate.Specifications;

public class FilterHomeRoomSpecification : Specification<HomeRoom, Guid>
{
    private readonly Guid? _facultyId;

    public FilterHomeRoomSpecification(Guid? facultyId)
    {
        _facultyId = facultyId;
    }
    
    public override Expression<Func<HomeRoom, bool>> ToExpression()
    {
        return x => (!_facultyId.HasValue) || (x.FacultyId == _facultyId);
    }
}