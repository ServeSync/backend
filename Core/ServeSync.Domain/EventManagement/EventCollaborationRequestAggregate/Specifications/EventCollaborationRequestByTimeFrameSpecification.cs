using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Specifications;

public class EventCollaborationRequestByTimeFrameSpecification : Specification<EventCollaborationRequest, Guid>
{
    private readonly DateTime? _startAt;
    private readonly DateTime? _endAt;
    
    public EventCollaborationRequestByTimeFrameSpecification(DateTime? startAt, DateTime? endAt)
    {
        _startAt = startAt;
        _endAt = endAt;
    }
    
    public override Expression<Func<EventCollaborationRequest, bool>> ToExpression()
    {
        return x => ((!_startAt.HasValue || x.StartAt.Date >= _startAt.Value.Date) && (!_endAt.HasValue || x.StartAt.Date <= _endAt.Value.Date)) ||
                    ((!_startAt.HasValue || x.EndAt.Date >= _startAt.Value.Date) && (!_endAt.HasValue || x.EndAt.Date <= _endAt.Value.Date));
    }
}
