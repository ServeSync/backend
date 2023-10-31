using Microsoft.AspNetCore.Authorization;
using ServeSync.API.Common.Enums;

namespace ServeSync.API.Authorization;

public class EventAccessControlAttribute : AuthorizeAttribute
{
    public static readonly string Prefix = "ServeSync.EventAccessControl";
    
    public EventAccessControlAttribute(EventSourceAccessControl source) : base($"{Prefix}.{source}")
    {
    }
}