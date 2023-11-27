using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Models;

public class TenantEntity<TKey> : Entity<TKey>, ITenantEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public Guid? TenantId { get; set; }
}

public class TenantEntity : Entity, ITenantEntity
{
    public Guid? TenantId { get; set; }
}