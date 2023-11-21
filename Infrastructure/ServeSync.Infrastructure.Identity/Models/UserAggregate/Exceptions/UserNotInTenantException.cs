using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

public class UserNotInTenantException : ResourceNotFoundException
{
    public UserNotInTenantException(string userId, Guid tenantId) 
        : base($"User with id {userId} is not in tenant with id {tenantId}.", ErrorCodes.UserNotInTenant)
    {
    }
}