namespace PBL6.Domain.SeedWorks.Models;

public interface IAggregateRoot<out TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
{
    
}

public interface IAggregateRoot : IAggregateRoot<Guid>
{
    
}