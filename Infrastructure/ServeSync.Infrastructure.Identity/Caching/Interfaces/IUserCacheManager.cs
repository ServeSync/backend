namespace ServeSync.Infrastructure.Identity.Caching.Interfaces;

public interface IUserCacheManager
{
    Task<IEnumerable<string>?> GetRolesAsync(string id, Guid tenantId);

    Task SetRolesAsync(string id, Guid tenantId, IEnumerable<string> roles);
}