namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface IFullAuditableAggregateRoot<out TKey> : IAuditableAggregateRoot<TKey>, IHasSoftDelete
    where TKey : IEquatable<TKey>
{
    
}

public interface IFullAuditableAggregateRoot : IFullAuditableAggregateRoot<Guid>
{
    
}
