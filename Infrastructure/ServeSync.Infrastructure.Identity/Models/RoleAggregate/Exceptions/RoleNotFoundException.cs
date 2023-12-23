using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;

public class RoleNotFoundException : ResourceNotFoundException
{
    public RoleNotFoundException(string id) : base("Role", nameof(ApplicationRole.Id),id, ErrorCodes.RoleNotFound)
    {
    }
    
    public RoleNotFoundException(string column, string value) : base("Role", column, value, ErrorCodes.RoleNotFound)
    {
    }
}