using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class RegisteredStudentInEventRoleSpecification : Specification<StudentEventRegister, Guid>
{
    private readonly Guid _eventRoleId;
    
    public RegisteredStudentInEventRoleSpecification(Guid eventRoleId)
    {
        _eventRoleId = eventRoleId;
    }
    
    public override Expression<Func<StudentEventRegister, bool>> ToExpression()
    {
        return x => x.EventRoleId == _eventRoleId;
    }
}