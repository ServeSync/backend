using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Models;

public class TenantAggregateRoot<TKey> : AggregateRoot<TKey>, ITenantAggregateRoot<TKey> where TKey : IEquatable<TKey>
{
    public Guid? TenantId { get; set; }
}

public class TenantAggregateRoot : AggregateRoot, ITenantAggregateRoot
{
    public Guid? TenantId { get; set; }
}