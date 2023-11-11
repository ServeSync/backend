using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Enums;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Specifications;

public class EventCollaborationRequestByStatusSpecification : Specification<EventCollaborationRequest, Guid>
{
    private readonly CollaborationRequestStatus _status;
    
    public EventCollaborationRequestByStatusSpecification(CollaborationRequestStatus status)
    {
        _status = status;
    }
    
    public override Expression<Func<EventCollaborationRequest, bool>> ToExpression()
    {
        if (_status == CollaborationRequestStatus.Expired)
        {
            return x => x.Status == CollaborationRequestStatus.Pending && x.StartAt.AddDays(-1) <= DateTime.UtcNow;
        }
        
        return x => x.Status == _status;
    }
}