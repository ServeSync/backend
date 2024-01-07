using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class EventRegisteredByStudentSpecification : Specification<Event, Guid>
{
    private readonly Guid _studentId;
    
    public EventRegisteredByStudentSpecification(Guid studentId)
    {
        _studentId = studentId;
    }
    
    public override Expression<Func<Event, bool>> ToExpression()
    {
        return x => x.Roles.Any(r => r.StudentEventRegisters.Any(s => s.StudentId == _studentId && s.Status == EventRegisterStatus.Approved));
    }
}