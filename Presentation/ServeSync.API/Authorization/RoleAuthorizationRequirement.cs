using Microsoft.AspNetCore.Authorization;

namespace ServeSync.API.Authorization;

public class RoleAuthorizationRequirement : IAuthorizationRequirement
{
    public string Role { get; set; }
    
    public RoleAuthorizationRequirement(string role)
    {
        Role = role;
    }
}