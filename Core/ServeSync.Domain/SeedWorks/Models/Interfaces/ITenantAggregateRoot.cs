namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface ITenantAggregateRoot<TKey> : IAggregateRoot<TKey>, ITenantEntity<TKey> 
    where TKey : IEquatable<TKey>
{
    
}

public interface ITenantAggregateRoot : ITenantAggregateRoot<Guid>, IAggregateRoot, ITenantEntity
{
    
}