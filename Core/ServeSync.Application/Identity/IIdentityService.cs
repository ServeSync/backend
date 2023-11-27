using ServeSync.Application.Identity.Dtos;

namespace ServeSync.Application.Identity;

public interface IIdentityService
{
    Task<IdentityUserDto?> GetByIdAsync(string userId); 
    
    Task<IdentityUserDto?> GetByUserNameAsync(string username);
    
    Task<IEnumerable<string>> GetPermissionsForUserAsync(string userId, Guid tenantId);

    Task<IEnumerable<string>> GetRolesAsync(string userId, Guid tenantId);

    Task<bool> HasPermissionAsync(string userId, Guid tenantId, string permission);

    Task<IdentityResult<IdentityUserDto>> CreateUserAsync(
        string fullname, 
        string username, 
        string avatarUrl, 
        string email, 
        string password, 
        string? phone = null, 
        Guid? externalId = null);

    Task<IdentityResult<IdentityUserDto>> CreateStudentAsync(
        string fullname, 
        string username, 
        string avatarUrl, 
        string email, 
        string password, 
        Guid studentId, 
        string? phone = null);

    Task<IdentityResult<IdentityUserDto>> CreateEventOrganizationContactAsync(
        string fullname, 
        string username, 
        string avatarUrl, 
        string email, 
        string password, 
        Guid contactId, 
        Guid tenantId,
        string? phone = null);
    
    Task<IdentityResult<IdentityUserDto>> CreateEventOrganizationAsync(
        string fullname, 
        string username, 
        string avatarUrl, 
        string email, 
        string password, 
        Guid organizationId,
        Guid tenantId,
        string? phone = null);
    
    Task<IdentityResult<bool>> UpdateAsync(string userId, string fullname, string email, string avatarUrl);
    
    Task<IdentityResult<bool>> UpdateUserNameAsync(string userId, string username);
    
    Task<IdentityResult<bool>> DeleteAsync(string userId);
    
    Task<bool> IsOrganizationContactAsync(string userId, Guid tenantId);
    
    Task<bool> IsEventOrganizationAsync(string userId, Guid tenantId);
    
    Task<IdentityResult<bool>> GrantToRoleAsync(string userId, string role, Guid tenantId);
    
    Task<IdentityResult<bool>> UnGrantFromRoleAsync(string userId, string role, Guid tenantId);
}