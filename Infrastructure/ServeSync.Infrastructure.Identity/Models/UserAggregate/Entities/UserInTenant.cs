using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

public record UserInTenant : ValueObject
{
    public Guid TenantId { get; set; }
    public string UserId { get; set; }
    
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
    
    public bool IsOwner { get; set; }
    
    public ApplicationUser? User { get; set; }
    public Tenant? Tenant { get; set; }
    
    public UserInTenant(Guid tenantId, string userId, string fullName, string avatarUrl, bool isOwner)
    {
        TenantId = tenantId;
        UserId = userId;
        FullName = fullName;
        AvatarUrl = avatarUrl;
        IsOwner = isOwner;
    }
    
    public void Update(string fullName, string avatarUrl)
    {
        FullName = fullName;
        AvatarUrl = avatarUrl;
    }

    private UserInTenant()
    {
    }
}