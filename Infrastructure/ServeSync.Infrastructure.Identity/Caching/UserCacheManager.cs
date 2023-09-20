using Microsoft.AspNetCore.Identity;
using ServeSync.Application.Caching;
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
    
    public Task<IEnumerable<string>?> GetRolesAsync(string id)
    {
        var key = KeyGenerator.Generate("UserRole", id);
        return _cacheService.GetRecordAsync<IEnumerable<string>?>(key);
    }
    
    public async Task SetRolesAsync(string id, IEnumerable<string> roles)
    {
        var key = KeyGenerator.Generate("UserRole", id);
        await _cacheService.SetRecordAsync(key, roles, TimeSpan.FromMinutes(30));
    }
}