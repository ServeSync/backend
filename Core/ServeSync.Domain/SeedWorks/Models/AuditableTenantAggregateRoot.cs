using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Models;

public class AuditableTenantAggregateRoot<TKey> : AuditableAggregateRoot<TKey>, IAuditableTenantEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public Guid? TenantId { get; set; }
}

public class AuditableTenantAggregateRoot : AuditableAggregateRoot, IAuditableTenantEntity
{
    public Guid? TenantId { get; set; }
}