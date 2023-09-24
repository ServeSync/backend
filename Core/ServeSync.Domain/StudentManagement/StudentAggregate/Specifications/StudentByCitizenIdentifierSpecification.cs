using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class StudentByCitizenIdentifierSpecification : Specification<Student, Guid>
{
    private readonly string _citizenId;

    public StudentByCitizenIdentifierSpecification(string citizenId)
    {
        _citizenId = citizenId;
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => x.CitizenId == _citizenId;
    }
}