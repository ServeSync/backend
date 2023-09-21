namespace ServeSync.Infrastructure.Identity.Caching.Interfaces;

public interface IUserCacheManager
{
    Task<IEnumerable<string>?> GetRolesAsync(string id);

    Task SetRolesAsync(string id, IEnumerable<string> roles);
}