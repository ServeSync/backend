using Microsoft.AspNetCore.Identity;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

public partial class ApplicationUser : IdentityUser
{
    public string FullName { get; private set; }
    public string AvatarUrl { get; private set; }
    public List<RefreshToken> RefreshToken { get; set; }

    public ApplicationUser(string fullname, string avatarUrl = null)
    {
        FullName = Guard.NotNullOrEmpty(fullname, nameof(FullName));
        AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? "https://static.thenounproject.com/png/5034901-200.png" : avatarUrl;
        RefreshToken = new List<RefreshToken>();
    }

    public void UpdateFullName(string fullName)
    {
        FullName = Guard.NotNullOrEmpty(fullName, nameof(FullName));
    }

    public void SetAvatar(string avatar)
    {
        AvatarUrl = Guard.NotNullOrWhiteSpace(avatar, nameof(AvatarUrl));
    }

    public void AddRefreshToken(string accessTokenId, string refreshToken, DateTime expireAt)
    {
        if (GetRefreshToken(accessTokenId, refreshToken) != null)
        {
            throw new RefreshTokeHasAlreadyAddedException(refreshToken);
        }
        
        RefreshToken.Add(new RefreshToken(accessTokenId, refreshToken, expireAt, Id));
    }

    public void UseRefreshToken(string accessTokenId, string refreshToken)
    {
        if (CanRefreshToken(accessTokenId, refreshToken))
        {
            RefreshToken.Remove(GetRefreshToken(accessTokenId, refreshToken));
        }
    }

    private bool CanRefreshToken(string accessTokenId, string refreshToken)
    {
        var token = GetRefreshToken(accessTokenId, refreshToken);
        
        if (token == null)
        {
            throw new RefreshTokenNotFoundException(refreshToken);
        }

        if (token.IsExpire())
        {
            throw new RefreshTokenHasAlreadyExpireException(refreshToken);
        }

        return true;
    }

    private RefreshToken? GetRefreshToken(string accessTokenId, string refreshToken)
    {
        return RefreshToken.FirstOrDefault(x => x.AccessTokenId == accessTokenId && x.Value == refreshToken);
    }

    private ApplicationUser()
    {
        RefreshToken = new List<RefreshToken>();
    }
}