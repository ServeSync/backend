using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class EventOrganizationContactByOrganizationSpecification : Specification<EventOrganizationContact, Guid>
{
    private readonly Guid _organizationId;
    
    public EventOrganizationContactByOrganizationSpecification(Guid organizationId)
    {
        _organizationId = organizationId;
    }
    
    public override Expression<Func<EventOrganizationContact, bool>> ToExpression()
    {
        return x => x.EventOrganizationId == _organizationId;
    }
}