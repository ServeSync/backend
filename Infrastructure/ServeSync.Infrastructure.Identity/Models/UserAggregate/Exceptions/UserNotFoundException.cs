using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class UserNotFoundException : ResourceNotFoundException
{
    public UserNotFoundException(string id) : base("User", id, ErrorCodes.UserNotFound)
    {
    }
}