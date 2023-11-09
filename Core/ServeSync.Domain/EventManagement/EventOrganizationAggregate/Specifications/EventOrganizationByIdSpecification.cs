using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class EventOrganizationByIdSpecification :Specification<EventOrganization, Guid>
{
    private readonly Guid _id;
    
    public EventOrganizationByIdSpecification(Guid id)
    {
        _id = id;
        
        AddInclude(x => x.Contacts!);
    }
    
    public override Expression<Func<EventOrganization, bool>> ToExpression()
    {
        return x => x.Id == _id;
    }
}