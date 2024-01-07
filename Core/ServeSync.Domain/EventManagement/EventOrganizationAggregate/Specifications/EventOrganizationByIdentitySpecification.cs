using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class EventOrganizationByIdentitySpecification : Specification<EventOrganization, Guid>
{
    private readonly string _identityId;
    
    public EventOrganizationByIdentitySpecification(string identityId)
    {
        _identityId = identityId;
    }
    
    public override Expression<Func<EventOrganization, bool>> ToExpression()
    {
        return _ => _.IdentityId == _identityId;
    }
}