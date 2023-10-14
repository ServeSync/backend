using Microsoft.AspNetCore.Authorization;

namespace ServeSync.API.Authorization;

public class HasRoleAttribute : AuthorizeAttribute
{
    public static readonly string Prefix = "ServeSync.Role";
    
    public HasRoleAttribute(string role) : base($"{Prefix}.{role}")
    {
    } 
}