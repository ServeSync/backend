using ServeSync.Application.Caching;
using ServeSync.Application.Common.Helpers;
using ServeSync.Infrastructure.Identity.Caching.Interfaces;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.Caching;

public class PermissionCacheManager : IPermissionCacheManager
{
    private readonly ICachingService _cacheService;
    
    public PermissionCacheManager(ICachingService cacheService)
    {
        _cacheService = cacheService;
    }
    
    public Task<IEnumerable<PermissionDto>?> GetForRoleAsync(string role)
    {
        var key = KeyGenerator.Generate("PermissionRole", role);
        return _cacheService.GetRecordAsync<IEnumerable<PermissionDto>?>(key);
    }

    public Task SetForRoleAsync(string role, IEnumerable<PermissionDto> permissions)
    {
        var key = KeyGenerator.Generate("PermissionRole", role);
        return _cacheService.SetRecordAsync<IEnumerable<PermissionDto>?>(key, permissions, TimeSpan.FromMinutes(30));
    }

    public Task RemoveForRoleAsync(string role)
    {
        var key = KeyGenerator.Generate("PermissionRole", role);
        return _cacheService.RemoveRecordAsync(key);
    }
}