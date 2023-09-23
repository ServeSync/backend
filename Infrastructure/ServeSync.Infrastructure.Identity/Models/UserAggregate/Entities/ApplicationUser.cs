using Microsoft.AspNetCore.Identity;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

public partial class ApplicationUser : IdentityUser
{
    public string FullName { get; private set; }
    public List<RefreshToken> RefreshToken { get; set; }

    public ApplicationUser(string fullname)
    {
        FullName = Guard.NotNullOrEmpty(fullname, nameof(FullName));
        RefreshToken = new List<RefreshToken>();
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
        
    }
}