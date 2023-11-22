using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class EventOrganizationContactByStatusSpecification : Specification<EventOrganizationContact, Guid>
{
    private readonly OrganizationStatus _status;
    
    public EventOrganizationContactByStatusSpecification(OrganizationStatus status)
    {
        _status = status;
    }
    
    public override Expression<Func<EventOrganizationContact, bool>> ToExpression()
    {
        return x => x.Status == _status;
    }
}