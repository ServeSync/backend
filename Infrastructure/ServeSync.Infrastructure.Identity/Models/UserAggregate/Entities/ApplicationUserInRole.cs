using Microsoft.AspNetCore.Identity;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

public class ApplicationUserInRole : IdentityUserRole<string>
{
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    
    public ApplicationRole? Role { get; set; }
    
    public ApplicationUserInRole(string userId, string roleId, Guid tenantId)
    {
        UserId = userId;
        RoleId = roleId;
        TenantId = tenantId;
    }
    
    public ApplicationUserInRole()
    {
    }
}