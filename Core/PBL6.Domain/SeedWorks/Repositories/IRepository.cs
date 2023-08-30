using PBL6.Domain.SeedWorks.Models;

namespace PBL6.Domain.SeedWorks.Repositories;

public interface IRepository<TAggregateRoot, TKey> : IReadOnlyRepository<TAggregateRoot, TKey>, ICommandRepository<TAggregateRoot, TKey> 
    where TAggregateRoot : IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    
}

public interface IRepository<TAggregateRoot> : IRepository<TAggregateRoot, Guid>
    where TAggregateRoot : IAggregateRoot
{
    
}