using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class EventOrganizationContactByIdentitySpecification : Specification<EventOrganizationContact, Guid>
{
    private readonly string _identityId;
    
    public EventOrganizationContactByIdentitySpecification(string identityId)
    {
        _identityId = identityId;
    }
    
    public override Expression<Func<EventOrganizationContact, bool>> ToExpression()
    {
        return _ => _.IdentityId == _identityId;
    }
}