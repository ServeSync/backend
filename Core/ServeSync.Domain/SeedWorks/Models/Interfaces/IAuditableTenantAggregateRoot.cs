namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface IAuditableTenantAggregateRoot<TKey> : IAuditableAggregateRoot<TKey>, ITenantAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    
}

public interface IAuditableTenantAggregateRoot : IAuditableTenantAggregateRoot<Guid>, IAuditableAggregateRoot, ITenantAggregateRoot
{
    
}