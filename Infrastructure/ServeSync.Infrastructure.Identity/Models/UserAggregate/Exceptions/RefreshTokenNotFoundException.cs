using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class RefreshTokenNotFoundException : ResourceNotFoundException
{
    public RefreshTokenNotFoundException(string refreshToken) 
        : base($"Refresh token '{refreshToken}' not found!", ErrorCodes.RefreshTokenNotFound)
    {
    }
}