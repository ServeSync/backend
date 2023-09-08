using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Domain.SeedWorks.Repositories;

public interface ISpecificationRepository<TAggregateRoot, TKey> 
    where TAggregateRoot : IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    Task<IList<TAggregateRoot>> GetPagedListAsync(int skip, int take, ISpecification<TAggregateRoot, TKey> specification, string? sorting = null);

    Task<IList<TAggregateRoot>> GetPagedListAsync(IPagingAndSortingSpecification<TAggregateRoot, TKey> specification);

    Task<TAggregateRoot?> FindAsync(ISpecification<TAggregateRoot, TKey> specification);

    Task<int> GetCountAsync(ISpecification<TAggregateRoot, TKey> specification);

    Task<bool> AnyAsync(ISpecification<TAggregateRoot, TKey> specification);
    
    Task<bool> AllAsync(ISpecification<TAggregateRoot, TKey> specification);
}

public interface ISpecificationRepository<TAggregateRoot> : ISpecificationRepository<TAggregateRoot, Guid>
    where TAggregateRoot : IAggregateRoot
{
}