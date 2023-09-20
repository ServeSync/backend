namespace ServeSync.Application.Identity;

public interface IIdentityService
{
    Task<IEnumerable<string>> GetPermissionsForUserAsync(string userId);
}