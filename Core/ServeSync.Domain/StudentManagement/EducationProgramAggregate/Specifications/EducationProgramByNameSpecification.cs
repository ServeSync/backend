using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.EducationProgramAggregate.Specifications;

public class EducationProgramByNameSpecification : Specification<EducationProgram, Guid>
{
    private readonly string _name;

    public EducationProgramByNameSpecification(string name)
    {
        _name = name;
    }
    
    public override Expression<Func<EducationProgram, bool>> ToExpression()
    {
        return x => x.Name == _name;
    }
}