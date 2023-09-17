using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class InvalidForgetPasswordTokenException : ResourceInvalidOperationException
{
    public InvalidForgetPasswordTokenException() 
        : base("Token is invalid!", ErrorCodes.InvalidForgetPasswordToken)
    {
    }
}