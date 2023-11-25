namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface ITenantEntity<out TKey> : IEntity<TKey>, IHasTenant where TKey : IEquatable<TKey>
{
    
}

public interface ITenantEntity : ITenantEntity<Guid>, IEntity
{
}