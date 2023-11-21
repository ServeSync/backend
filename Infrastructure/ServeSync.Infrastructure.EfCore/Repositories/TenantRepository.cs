using Microsoft.EntityFrameworkCore;
using ServeSync.Infrastructure.EfCore.Repositories.Base;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class TenantRepository : EfCoreRepository<Tenant, Guid>, ITenantRepository
{
    public TenantRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<Tenant>> FindByUserAsync(string userId)
    {
        var userInTenantQueryable = DbContext.Set<UserInTenant>().Where(x => x.UserId == userId);
        
        return await DbSet.Where(x => userInTenantQueryable.Any(y => y.TenantId == x.Id))
                    .ToListAsync();
    }
}