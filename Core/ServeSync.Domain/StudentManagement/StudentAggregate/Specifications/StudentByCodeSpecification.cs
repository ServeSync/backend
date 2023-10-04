using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class StudentByCodeSpecification : Specification<Student, Guid>
{
    private readonly string _code;

    public StudentByCodeSpecification(string code)
    {
        _code = code;
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => x.Code == _code;
    }
}