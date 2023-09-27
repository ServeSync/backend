using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class StudentByIdentitySpecification : Specification<Student, Guid>
{
    private readonly string _identityId;

    public StudentByIdentitySpecification(string identityId)
    {
        _identityId = identityId;
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => x.IdentityId == _identityId;
    }
}