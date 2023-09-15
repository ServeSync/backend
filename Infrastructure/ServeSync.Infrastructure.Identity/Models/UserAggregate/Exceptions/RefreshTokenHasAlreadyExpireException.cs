using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class RefreshTokenHasAlreadyExpireException : ResourceInvalidOperationException
{
    public RefreshTokenHasAlreadyExpireException(string refreshToken) 
        : base($"Refresh token '{refreshToken}' has already expired!", ErrorCodes.RefreshTokenAlreadyExpire)
    {
    }
}