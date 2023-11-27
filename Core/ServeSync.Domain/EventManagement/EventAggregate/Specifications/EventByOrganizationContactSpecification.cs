using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventAggregate.Specifications;

public class EventByOrganizationContactSpecification : Specification<Event, Guid>
{
    private readonly Guid _organizationContactId;
    private readonly string _createdBy;
    private readonly Guid _tenantId;
    
    public EventByOrganizationContactSpecification(Guid organizationContactId, string createdBy, Guid tenantId)
    {
        _organizationContactId = organizationContactId;
        _createdBy = createdBy;
        _tenantId = tenantId;
        
        AddInclude(x => x.Organizations);
        AddInclude("Organizations.Organization.Contacts");
    }
    
    public override Expression<Func<Event, bool>> ToExpression()
    {
        return x => x.Organizations.Any(y => y.Organization!.Contacts.Any(z => z.Id == _organizationContactId))
                         || (x.CreatedBy == _createdBy && x.TenantId == _tenantId);
    }
}