using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Specifications;

public class EventCollaborationRequestByTypeSpecification : Specification<EventCollaborationRequest, Guid>
{
    private readonly EventType _type;

    public EventCollaborationRequestByTypeSpecification(EventType type)
    {
        _type = type;
    }

    public override Expression<Func<EventCollaborationRequest, bool>> ToExpression()
    {
        return x => x.Type == _type;
    }
}