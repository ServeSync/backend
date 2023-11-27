using ServeSync.Application.Identity.Dtos;

namespace ServeSync.Application.Identity;

public interface ITenantService
{
    Task<IdentityResult<TenantDto>> CreateAsync(string name, string avatarUrl);
    
    Task UpdateAsync(Guid tenantId, string name, string avatarUrl);
    
    Task DeleteAsync(Guid tenantId);
    
    Task AddUserToTenantAsync(string userId, string fullname, string avatarUrl, bool isOwner, Guid tenantId);

    Task UpdateUserInTenantAsync(string userId, Guid tenantId, string fullname, string avatarUrl);
    
    Task RemoveUserFromTenantAsync(string userId, Guid tenantId);
}