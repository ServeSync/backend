using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class StudentByEducationProgramSpecification : Specification<Student, Guid>
{
    private readonly Guid _educationProgramId;
    
    public StudentByEducationProgramSpecification(Guid educationProgramId)
    {
        _educationProgramId = educationProgramId;
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => x.EducationProgramId == _educationProgramId;
    }
}