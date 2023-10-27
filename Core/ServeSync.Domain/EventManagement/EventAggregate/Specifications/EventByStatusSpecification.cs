using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class EventByStatusSpecification : Specification<Event, Guid>
{
    private readonly EventStatus _status;
    private readonly DateTime _dateTime;
    
    public EventByStatusSpecification(EventStatus status, DateTime dateTime)
    {
        _status = status;
        _dateTime = dateTime;
        
        AddInclude(x => x.RegistrationInfos);
        AddInclude(x => x.AttendanceInfos);
    }
    
    public override Expression<Func<Event, bool>> ToExpression()
    {
        if (_status == EventStatus.Attendance)
        {
            return x => x.Status == EventStatus.Approved && x.StartAt <= _dateTime && x.EndAt >= _dateTime && x.AttendanceInfos.Any(y => _dateTime >= y.StartAt && _dateTime <= y.EndAt);
        }
        else if (_status == EventStatus.Happening)
        {
            return x => x.Status == EventStatus.Approved && x.StartAt <= _dateTime && x.EndAt >= _dateTime;
        }
        else if (_status == EventStatus.Registration)
        {
            return x => x.Status == EventStatus.Approved && x.StartAt >= _dateTime && x.RegistrationInfos.Any(y => _dateTime >= y.StartAt && _dateTime <= y.EndAt);
        }
        else if (_status == EventStatus.Upcoming)
        {
            return x => x.Status == EventStatus.Approved && x.StartAt >= _dateTime;
        }
        else if (_status == EventStatus.Done)
        {
            return x => x.Status == EventStatus.Approved && x.EndAt <= _dateTime;
        }
        else if (_status == EventStatus.Expired)
        {
            return x => x.Status == EventStatus.Pending && x.StartAt <= _dateTime;
        }
        else if (_status == EventStatus.ClosedRegistration)
        {
            return x => x.Status == EventStatus.Approved && x.RegistrationInfos.All(y => _dateTime >= y.EndAt);
        }

        return x => x.Status == _status;
    }
}