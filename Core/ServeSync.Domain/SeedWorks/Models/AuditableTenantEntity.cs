using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Models;

public class AuditableTenantEntity<TKey> : AuditableEntity<TKey>, IAuditableTenantEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public Guid? TenantId { get; set; }
}

public class AuditableTenantEntity : AuditableEntity, IAuditableTenantEntity
{
    public Guid? TenantId { get; set; }
}