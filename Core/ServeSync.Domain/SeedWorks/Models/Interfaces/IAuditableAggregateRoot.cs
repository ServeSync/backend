namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface IAuditableAggregateRoot<out TKey> : IAggregateRoot<TKey>, IHasAuditable
    where TKey : IEquatable<TKey>
{
    
}

public interface IAuditableAggregateRoot : IAuditableAggregateRoot<Guid>, IAggregateRoot, IHasAuditable
{
    
}