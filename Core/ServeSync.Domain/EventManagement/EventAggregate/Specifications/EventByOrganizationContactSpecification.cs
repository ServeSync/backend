using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class EventByOrganizationContactSpecification : Specification<Event, Guid>
{
    private readonly Guid _organizationContactId;
    
    public EventByOrganizationContactSpecification(Guid organizationContactId)
    {
        _organizationContactId = organizationContactId;
        
        AddInclude(x => x.Organizations);
        AddInclude("Organizations.Organization.Contacts");
    }
    
    public override Expression<Func<Event, bool>> ToExpression()
    {
        return x => x.Organizations.Any(y => y.Organization!.Contacts.Any(z => z.Id == _organizationContactId));
    }
}