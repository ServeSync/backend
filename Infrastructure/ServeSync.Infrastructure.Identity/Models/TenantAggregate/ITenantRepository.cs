using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.TenantAggregate;

public interface ITenantRepository : IRepository<Tenant, Guid>
{
    
}