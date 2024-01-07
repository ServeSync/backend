using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class EventAttendanceByTimeFrameSpecification : Specification<StudentEventAttendance, Guid>
{
    private readonly DateTime? _startAt;
    private readonly DateTime? _endAt;
    
    public EventAttendanceByTimeFrameSpecification(DateTime? startAt, DateTime? endAt)
    {
        _startAt = startAt;
        _endAt = endAt;
    }
    
    public override Expression<Func<StudentEventAttendance, bool>> ToExpression()
    {
        return x => ((!_startAt.HasValue || x.AttendanceAt.Date >= _startAt.Value.Date) &&
                     (!_endAt.HasValue || x.AttendanceAt.Date <= _endAt.Value.Date));
    }
}