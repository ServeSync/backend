using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;

public class PermissionHasAlreadyGrantedToRoleException : ResourceAlreadyExistException
{
    public PermissionHasAlreadyGrantedToRoleException(string roleId, Guid permissionId) 
        : base($"Permission with id '{permissionId}' has already been granted to role with id '{roleId}'!", ErrorCodes.PermissionHasAlreadyGrantedToRole)
    {
    }
}