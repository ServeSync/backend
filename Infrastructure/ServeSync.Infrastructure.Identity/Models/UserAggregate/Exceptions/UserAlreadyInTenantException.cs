using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class UserAlreadyInTenantException : ResourceAlreadyExistException
{
    public UserAlreadyInTenantException(string userId, Guid tenantId) 
        : base($"User with id {userId} is already in tenant with id {tenantId}.", ErrorCodes.UserAlreadyInTenant)
    {
    }
}