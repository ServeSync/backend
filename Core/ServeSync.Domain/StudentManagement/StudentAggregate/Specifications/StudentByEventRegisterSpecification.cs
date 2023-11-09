using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class StudentByEventRegisterSpecification : Specification<Student, Guid>
{
    private readonly Guid _eventRegisterId;
    
    public StudentByEventRegisterSpecification(Guid eventRegisterId)
    {
        _eventRegisterId = eventRegisterId;
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => x.EventRegisters.Any(y => y.Id == _eventRegisterId);
    }
}