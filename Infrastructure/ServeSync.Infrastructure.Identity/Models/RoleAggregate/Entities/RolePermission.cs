using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

public record RolePermission : ValueObject
{
    public string RoleId { get; set; }
    public Guid PermissionId { get; set; }

    public RolePermission(string roleId, Guid permissionId)
    {
        RoleId = Guard.NotNullOrEmpty(roleId, nameof(RoleId));
        PermissionId = Guard.NotNull(permissionId, nameof(PermissionId));
    }
}