using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class EventRegisterByTimeFrameSpecification : Specification<StudentEventRegister, Guid>
{
    private readonly DateTime? _startAt;
    private readonly DateTime? _endAt;
    
    public EventRegisterByTimeFrameSpecification(DateTime? startAt, DateTime? endAt)
    {
        _startAt = startAt;
        _endAt = endAt;
    }
    
    public override Expression<Func<StudentEventRegister, bool>> ToExpression()
    {
        return x => ((!_startAt.HasValue || x.Created.Date >= _startAt.Value.Date) &&
                     (!_endAt.HasValue || x.Created.Date <= _endAt.Value.Date));
    }
}