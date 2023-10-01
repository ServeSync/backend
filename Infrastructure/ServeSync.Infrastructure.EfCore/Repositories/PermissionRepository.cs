using Microsoft.EntityFrameworkCore;
using ServeSync.Infrastructure.EfCore.Repositories.Base;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Specifications;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class PermissionRepository : EfCoreRepository<ApplicationPermission, Guid>, IPermissionRepository
{
    public PermissionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<ApplicationPermission>> FilterAsync(string name)
    {
        return await GetQueryable(new PermissionByNameSpecification(name)).OrderBy(x => x.Name).ToListAsync();
    }
}