using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class EventByStatusSpecification : Specification<Event, Guid>
{
    private readonly EventStatus _status;
    
    public EventByStatusSpecification(EventStatus status)
    {
        _status = status;
    }
    
    public override Expression<Func<Event, bool>> ToExpression()
    {
        if (_status == EventStatus.Happening)
        {
            return x => x.Status == EventStatus.Approved && x.StartAt <= DateTime.Now && x.EndAt >= DateTime.Now;
        }
        else if (_status == EventStatus.Upcoming)
        {
            return x => x.Status == EventStatus.Approved && x.StartAt >= DateTime.Now;
        }
        else if (_status == EventStatus.Done)
        {
            return x => x.Status == EventStatus.Approved && x.EndAt <= DateTime.Now;
        }
        else
        {
            return x => x.Status == _status;
        }
    }
}