using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Exceptions;

public class PermissionNotFoundException : ResourceNotFoundException
{
    public PermissionNotFoundException(Guid id) : base("Permission", id, ErrorCodes.PermissionNotFound)
    {
    }
}