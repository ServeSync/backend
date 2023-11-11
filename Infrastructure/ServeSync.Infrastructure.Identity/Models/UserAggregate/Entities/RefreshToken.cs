using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

public class RefreshToken : Entity
{
    public string AccessTokenId { get; private set; }
    public string Value { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    
    public string UserId { get; private set; }
    public ApplicationUser User { get; private set; }

    public RefreshToken(string accessTokenId, string refreshToken, DateTime expiresAt, string userId)
    {
        AccessTokenId = Guard.NotNullOrEmpty(accessTokenId, nameof(AccessTokenId));
        Value = Guard.NotNullOrEmpty(refreshToken, nameof(RefreshToken));
        ExpiresAt = Guard.NotNull(expiresAt, nameof(ExpiresAt));
        UserId = Guard.NotNullOrEmpty(userId, nameof(UserId));
    }

    private RefreshToken()
    {
        
    }

    public bool IsExpire()
    {
        return ExpiresAt < DateTime.UtcNow;
    }
}