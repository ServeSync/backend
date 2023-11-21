using ServeSync.Infrastructure.EfCore.Repositories.Base;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class TenantRepository : EfCoreRepository<Tenant, Guid>, ITenantRepository
{
    public TenantRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}