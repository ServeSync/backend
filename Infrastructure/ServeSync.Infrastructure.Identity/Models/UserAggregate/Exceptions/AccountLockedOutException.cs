using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class AccountLockedOutException: ResourceAccessDeniedException
{
    public AccountLockedOutException() : base("This account has been locked out!", ErrorCodes.AccountLockedOut)
    {
    }
}