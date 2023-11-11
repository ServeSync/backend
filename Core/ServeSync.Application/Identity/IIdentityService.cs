using ServeSync.Application.Identity.Dtos;

namespace ServeSync.Application.Identity;

public interface IIdentityService
{
    Task<IdentityUserDto?> GetByIdAsync(string userId); 
    
    Task<IEnumerable<string>> GetPermissionsForUserAsync(string userId);

    Task<IEnumerable<string>> GetRolesAsync(string userId);

    Task<bool> HasPermissionAsync(string userId, string permission);

    Task<IdentityResult<IdentityUserDto>> CreateUserAsync(string fullname, string username, string avatarUrl, string email, string password, string? phone = null, Guid? externalId = null);

    Task<IdentityResult<IdentityUserDto>> CreateStudentAsync(string fullname, string username, string avatarUrl, string email, string password, Guid studentId, string? phone = null);

    Task<IdentityResult<IdentityUserDto>> CreateEventOrganizationContactAsync(string fullname, string username, string avatarUrl, string email, string password, Guid contactId, string? phone = null);
    
    Task<IdentityResult<IdentityUserDto>> CreateEventOrganizationAsync(string fullname, string username, string avatarUrl, string email, string password, Guid organizationId, string? phone = null);
    
    Task<IdentityResult<bool>> UpdateAsync(string userId, string fullname, string email, string avatarUrl);
    
    Task<IdentityResult<bool>> UpdateUserNameAsync(string userId, string username);
    
    Task<IdentityResult<bool>> DeleteAsync(string userId);
    
    Task<IdentityResult<bool>> GrantToRoleAsync(string userId, string role);
    
    Task<IdentityResult<bool>> UnGrantFromRoleAsync(string userId, string role);
}