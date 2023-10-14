using Microsoft.AspNetCore.Authorization;

namespace ServeSync.API.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(permission)
    {
    } 
}