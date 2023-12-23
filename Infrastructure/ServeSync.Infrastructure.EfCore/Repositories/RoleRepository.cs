using Microsoft.EntityFrameworkCore;
using ServeSync.Infrastructure.EfCore.Repositories.Base;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class RoleRepository : EfCoreRepository<ApplicationRole, string>, IRoleRepository
{
    public RoleRepository(AppDbContext dbContext) : base(dbContext)
    {
        AddInclude(x => x.Permissions);
    }

    public Task<ApplicationRole?> FindByNameAsync(string name)
    {
        return GetQueryable().FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<IList<ApplicationRole>> FindByUserAsync(string userId, Guid tenantId)
    {
        var roleQueryable = DbContext.Set<ApplicationUserInRole>()
            .Where(x => x.UserId == userId && x.TenantId == tenantId);
        return await GetQueryable().Where(x => roleQueryable.Any(y => y.RoleId == x.Id)).ToListAsync();
    }
}