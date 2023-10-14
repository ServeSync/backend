using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class EventOrganizationRepository : EfCoreRepository<EventOrganization>, IEventOrganizationRepository
{
    public EventOrganizationRepository(AppDbContext dbContext) : base(dbContext)
    {
        AddInclude(x => x.Contacts);
    }
}