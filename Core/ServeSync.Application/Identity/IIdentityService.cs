using ServeSync.Application.Identity.Dtos;

namespace ServeSync.Application.Identity;

public interface IIdentityService
{
    Task<IEnumerable<string>> GetPermissionsForUserAsync(string userId);

    Task<IEnumerable<string>> GetRolesAsync(string userId);

    Task<bool> HasPermissionAsync(string userId, string permission);

    Task<IdentityResult<IdentityUserDto>> CreateUserAsync(string fullname, string username, string email, string password, string? phone = null);

    Task<IdentityResult<bool>> DeleteAsync(string userId);
    
    Task<IdentityResult<bool>> GrantToRoleAsync(string userId, string role);
    
    Task<IdentityResult<bool>> UnGrantFromRoleAsync(string userId, string role);
}