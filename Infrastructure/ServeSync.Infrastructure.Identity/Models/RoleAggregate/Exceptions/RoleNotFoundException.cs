using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;

public class RoleNotFoundException : ResourceNotFoundException
{
    public RoleNotFoundException(string id) : base("Role", id, ErrorCodes.RoleNotFound)
    {
    }
}