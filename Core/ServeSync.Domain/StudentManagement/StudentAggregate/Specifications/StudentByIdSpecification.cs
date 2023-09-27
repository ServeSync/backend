using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class StudentByIdSpecification : Specification<Student, Guid>
{
    private readonly Guid _id;

    public StudentByIdSpecification(Guid id)
    {
        _id = id;
        
        AddInclude(x => x.HomeRoom!);
        AddInclude("HomeRoom.Faculty");
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => x.Id == _id;
    }
}