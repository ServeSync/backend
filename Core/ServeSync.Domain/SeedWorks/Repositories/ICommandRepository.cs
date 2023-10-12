using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Repositories;

public interface ICommandRepository<in TAggregate, TKey> 
    where TAggregate : class, IAggregateRoot<TKey> 
    where TKey : IEquatable<TKey>
{
    Task InsertAsync(TAggregate entity);

    Task InsertRangeAsync(IEnumerable<TAggregate> entities);

    void Insert(TAggregate entity);

    void Delete(TAggregate entity);

    void Update(TAggregate entity);
}

public interface ICommandRepository<in TAggregate> : ICommandRepository<TAggregate, Guid>
    where TAggregate : class, IAggregateRoot
{
}