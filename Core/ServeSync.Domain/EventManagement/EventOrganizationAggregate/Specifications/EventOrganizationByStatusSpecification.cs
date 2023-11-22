using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class EventOrganizationByStatusSpecification : Specification<EventOrganization, Guid>
{
    private readonly OrganizationStatus _status;
    
    public EventOrganizationByStatusSpecification(OrganizationStatus status)
    {
        _status = status;
    }
    
    public override Expression<Func<EventOrganization, bool>> ToExpression()
    {
        return x => x.Status == _status;
    }
}