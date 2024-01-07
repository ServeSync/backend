using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.Common.Helpers;
using ServeSync.Infrastructure.Identity.Caching.Interfaces;

namespace ServeSync.Infrastructure.Identity.Caching;

public class UserCacheManager : IUserCacheManager
{
    private readonly ICachingService _cacheService;
    
    public UserCacheManager(ICachingService cacheService)
    {
        _cacheService = cacheService;
    }
    
    public Task<IEnumerable<string>?> GetRolesAsync(string id, Guid tenantId)
    {
        var key = KeyGenerator.Generate("UserRole", $"{id}{tenantId}");
        return _cacheService.GetRecordAsync<IEnumerable<string>?>(key);
    }

    public Task RemoveRolesAsync(string id, Guid tenantId)
    {
        var key = KeyGenerator.Generate("UserRole", $"{id}{tenantId}");
        return _cacheService.RemoveRecordAsync(key);
    }

    public async Task SetRolesAsync(string id, Guid tenantId, IEnumerable<string> roles)
    {
        var key = KeyGenerator.Generate("UserRole", $"{id}{tenantId}");
        await _cacheService.SetRecordAsync(key, roles, TimeSpan.FromMinutes(30));
    }
}