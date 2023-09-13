using Microsoft.EntityFrameworkCore;
using ServeSync.Infrastructure.EfCore.Repositories.Base;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

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
}