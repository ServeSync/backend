using Microsoft.AspNetCore.Authorization;
using ServeSync.API.Common.Enums;

namespace ServeSync.API.Authorization;

public class EventAccessControlRequirement : IAuthorizationRequirement
{
    public EventSourceAccessControl Source { get; set; }
    
    public EventAccessControlRequirement(EventSourceAccessControl source)
    {
        Source = source;
    }
}