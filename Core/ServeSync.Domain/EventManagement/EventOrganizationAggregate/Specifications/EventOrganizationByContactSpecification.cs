using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class EventOrganizationByContactSpecification : Specification<EventOrganization, Guid>
{
    private readonly Guid _contactId;
    
    public EventOrganizationByContactSpecification(Guid contactId)
    {
        _contactId = contactId;
        
        AddInclude(x => x.Contacts);
    }
    
    public override Expression<Func<EventOrganization, bool>> ToExpression()
    {
        return x => x.Contacts.Any(y => y.Id == _contactId);
    }
}