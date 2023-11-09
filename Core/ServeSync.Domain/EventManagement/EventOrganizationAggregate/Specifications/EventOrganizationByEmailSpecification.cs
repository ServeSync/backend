using System.Linq.Expressions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

public class EventOrganizationByEmailSpecification : Specification<EventOrganization, Guid>
{
    private readonly string _email;
    
    public EventOrganizationByEmailSpecification(string email)
    {
        _email = email;
    }
    
    public override Expression<Func<EventOrganization, bool>> ToExpression()
    {
        return x => x.Email == _email;
    }
}