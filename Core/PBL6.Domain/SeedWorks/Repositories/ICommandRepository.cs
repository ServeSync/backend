using PBL6.Domain.SeedWorks.Models;
using PBL6.Domain.SeedWorks.Models.Interfaces;

namespace PBL6.Domain.SeedWorks.Repositories;

public interface ICommandRepository<in TAggregate, TKey> 
    where TAggregate : IAggregateRoot<TKey> 
    where TKey : IEquatable<TKey>
{
    Task InsertAsync(TAggregate entity);

    Task InsertRangeAsync(IEnumerable<TAggregate> entities);

    void Insert(TAggregate entity);

    void Delete(TAggregate entity);

    void Update(TAggregate entity);
}

public interface ICommandRepository<in TAggregate> : ICommandRepository<TAggregate, Guid>
    where TAggregate : IAggregateRoot
{
}