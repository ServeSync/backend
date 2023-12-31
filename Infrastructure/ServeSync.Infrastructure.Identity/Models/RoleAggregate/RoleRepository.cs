﻿using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate;

public interface IRoleRepository : IRepository<ApplicationRole, string>
{
    Task<ApplicationRole?> FindByNameAsync(string name);
    
    Task<IList<ApplicationRole>> FindByUserAsync(string userId, Guid tenantId);
}