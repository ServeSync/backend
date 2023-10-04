using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.PermissionAggregate;

public interface IPermissionRepository : IRepository<ApplicationPermission, Guid>
{
    Task<IList<ApplicationPermission>> FilterAsync(string name);
}