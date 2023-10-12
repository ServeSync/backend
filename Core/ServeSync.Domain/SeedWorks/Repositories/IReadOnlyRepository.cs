using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Repositories;

public interface IReadOnlyRepository<TAggregateRoot, TKey> : IBasicReadOnlyRepository<TAggregateRoot, TKey> 
    where TAggregateRoot : class, IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
}

public interface IReadOnlyRepository<TAggregate> : IReadOnlyRepository<TAggregate, Guid>
    where TAggregate : class, IAggregateRoot
{
}