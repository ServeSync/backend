using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class StudentByEmailSpecification : Specification<Student, Guid>
{
    private readonly string _email;
    
    public StudentByEmailSpecification(string email)
    {
        _email = email;
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => x.Email == _email;
    }
}