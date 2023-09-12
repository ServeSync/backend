using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class UserNameOrEmailNotFoundException : ResourceNotFoundException
{
    public UserNameOrEmailNotFoundException(string userNameOrEmail) : base($"User with username or email '{userNameOrEmail}' does not exist!", ErrorCodes.UserNameOrEmailNotFound)
    {
    }
}