using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class EventRegisterByStatusSpecification : Specification<StudentEventRegister, Guid>
{
    private readonly EventRegisterStatus _status;
    
    public EventRegisterByStatusSpecification(EventRegisterStatus status)
    {
        _status = status;
    }
    
    public override Expression<Func<StudentEventRegister, bool>> ToExpression()
    {
        return x => x.Status == _status;
    }
}