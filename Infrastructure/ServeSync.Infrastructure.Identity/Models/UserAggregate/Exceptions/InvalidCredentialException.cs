using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class InvalidCredentialException: ResourceNotFoundException
{
    public InvalidCredentialException() : base("Account with provided information does not exist!", ErrorCodes.InvalidCredential)
    {
    }
}