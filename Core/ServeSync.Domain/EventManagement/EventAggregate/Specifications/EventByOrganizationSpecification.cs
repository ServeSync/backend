using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class EventByOrganizationSpecification : Specification<Event, Guid>
{
    private readonly Guid _organizationId;
    private readonly string _identityId;
    
    public EventByOrganizationSpecification(Guid organizationId, string identityId)
    {
        _organizationId = organizationId;
        _identityId = identityId;
        
        AddInclude(x => x.Organizations);
    }
    
    public override Expression<Func<Event, bool>> ToExpression()
    {
        return x => x.RepresentativeOrganizationId == _organizationId
                         || x.Organizations.Any(y => y.OrganizationId == _organizationId)
                         || x.CreatedBy == _identityId ;
    }
}