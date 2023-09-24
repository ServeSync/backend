using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class StudentByFacultySpecification : Specification<Student, Guid>
{
    private readonly Guid _facultyId;

    public StudentByFacultySpecification(Guid facultyId)
    {
        _facultyId = facultyId;
        
        AddInclude(x => x.HomeRoom);
        AddInclude("HomeRoom.Faculty");
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => x.HomeRoom.FacultyId == _facultyId;
    }
}