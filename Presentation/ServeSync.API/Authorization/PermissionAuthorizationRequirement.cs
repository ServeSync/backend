using Microsoft.AspNetCore.Authorization;

namespace ServeSync.API.Authorization;

public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public string Permission { get; set; }
    
    public PermissionAuthorizationRequirement(string permission)
    {
        Permission = permission;
    }
}