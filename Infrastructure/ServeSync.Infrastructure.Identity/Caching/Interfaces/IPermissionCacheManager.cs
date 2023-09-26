using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.Caching.Interfaces;

public interface IPermissionCacheManager
{
    Task<IEnumerable<PermissionDto>?> GetForRoleAsync(string role);
    
    Task SetForRoleAsync(string role, IEnumerable<PermissionDto> permissions);
    
    Task RemoveForRoleAsync(string role);
}