using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class RefreshTokeHasAlreadyAddedException : ResourceAlreadyExistException
{
    public RefreshTokeHasAlreadyAddedException(string refreshToken) 
        : base($"Refresh token '{refreshToken}' for access token has already added!", ErrorCodes.RefreshTokenAlreadyAdded)
    {
    }
}