using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;

public class PermissionHasNotGrantedToRoleException : ResourceNotFoundException
{
    public PermissionHasNotGrantedToRoleException(string roleId, Guid permissionId) 
        : base($"Permission with id '{permissionId}' has not been granted to role with id '{roleId}' yet!", ErrorCodes.PermissionHasNotGrantedToRole)
    {
    }
}