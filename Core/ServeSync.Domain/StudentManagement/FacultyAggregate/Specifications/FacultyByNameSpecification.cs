using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.FacultyAggregate.Specifications;

public class FacultyByNameSpecification : Specification<Faculty, Guid>
{
    private readonly string _name;

    public FacultyByNameSpecification(string name)
    {
        _name = name;
    }
    
    public override Expression<Func<Faculty, bool>> ToExpression()
    {
        return x => x.Name == _name;
    }
}