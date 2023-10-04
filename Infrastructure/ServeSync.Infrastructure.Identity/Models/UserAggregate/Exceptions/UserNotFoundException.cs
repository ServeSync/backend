using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class UserNotFoundException : ResourceNotFoundException
{
    public UserNotFoundException(string id) 
        : base("User", nameof(ApplicationUser.Id), id, ErrorCodes.UserNotFound)
    {
    }
}