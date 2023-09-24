using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Models;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey> where TKey : IEquatable<TKey>
{
    
}

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    
}