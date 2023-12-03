using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Domain.SeedWorks.Repositories;

public interface ISpecificationRepository<TEntity, TKey> 
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    Task<IList<TEntity>> FilterAsync(ISpecification<TEntity, TKey> specification);
    
    Task<IList<TEntity>> GetPagedListAsync(int skip, int take, ISpecification<TEntity, TKey> specification, string? sorting = null);

    Task<IList<TEntity>> GetPagedListAsync(IPagingAndSortingSpecification<TEntity, TKey> specification);
    
    Task<IList<TOut>> GetPagedListAsync<TOut>(IPagingAndSortingSpecification<TEntity, TKey> specification, IProjection<TEntity, TKey, TOut> projection);

    Task<TEntity?> FindAsync(ISpecification<TEntity, TKey> specification);

    Task<int> GetCountAsync(ISpecification<TEntity, TKey> specification);

    Task<bool> AnyAsync(ISpecification<TEntity, TKey> specification);
    
    Task<bool> AllAsync(ISpecification<TEntity, TKey> specification);
}

public interface ISpecificationRepository<TEntity> : ISpecificationRepository<TEntity, Guid>
    where TEntity : IEntity
{
}