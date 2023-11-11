using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class EventOrganizationByNameSpecification : Specification<EventOrganization, Guid>
{
    private readonly string _name;
    
    public EventOrganizationByNameSpecification(string name)
    {
        _name = name;
    }
    
    public override Expression<Func<EventOrganization, bool>> ToExpression()
    {
        return x => x.Name == _name;
    }
}