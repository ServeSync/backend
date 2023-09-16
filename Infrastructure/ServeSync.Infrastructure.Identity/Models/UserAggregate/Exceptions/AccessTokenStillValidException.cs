using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class AccessTokenStillValidException : ResourceInvalidOperationException
{
    public AccessTokenStillValidException() 
        : base("Access token still valid!", ErrorCodes.AccessTokenStillValid)
    {
    }
}