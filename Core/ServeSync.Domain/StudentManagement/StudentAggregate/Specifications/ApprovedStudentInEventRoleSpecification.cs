using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class ApprovedStudentInEventRoleSpecification : Specification<StudentEventRegister, Guid>
{
    private readonly Guid _eventRoleId;
    
    public ApprovedStudentInEventRoleSpecification(Guid eventRoleId)
    {
        _eventRoleId = eventRoleId;
    }
    
    public override Expression<Func<StudentEventRegister, bool>> ToExpression()
    {
        return x => x.EventRoleId == _eventRoleId && x.Status == EventRegisterStatus.Approved;
    }
}