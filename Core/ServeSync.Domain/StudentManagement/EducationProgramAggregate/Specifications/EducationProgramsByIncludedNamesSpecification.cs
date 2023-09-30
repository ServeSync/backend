using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.EducationProgramAggregate.Specifications;

public class EducationProgramsByIncludedNamesSpecification : Specification<EducationProgram, Guid>
{
    private readonly IEnumerable<string> _names;
    
    public EducationProgramsByIncludedNamesSpecification(IEnumerable<string> names)
    {
        _names = names;
    }
    
    public override Expression<Func<EducationProgram, bool>> ToExpression()
    {
        return x => _names.Contains(x.Name);
    }
}