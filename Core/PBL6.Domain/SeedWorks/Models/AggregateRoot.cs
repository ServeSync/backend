namespace PBL6.Domain.SeedWorks.Models;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey> where TKey : IEquatable<TKey>
{
    
}

public abstract class AggregateRoot : AggregateRoot<Guid>
{
    
}