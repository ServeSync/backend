using Microsoft.EntityFrameworkCore;
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

    public Task<EventOrganization?> FindByEmailAsync(string email)
    {
        return GetQueryable().FirstOrDefaultAsync(x => x.Email == email);
    }
}