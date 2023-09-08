using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Specifications.Interfaces;

public interface IPagingAndSortingSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> 
    where TEntity : IEntity<TKey> 
    where TKey : IEquatable<TKey> 
{
    int Take { get; set; }
    int Skip { get; set; }
    string Sorting { get; set; }

    string BuildSorting();

    new IPagingAndSortingSpecification<TEntity, TKey> And(ISpecification<TEntity, TKey> specification);
}

public interface IPagingAndSortingSpecification<TEntity> : IPagingAndSortingSpecification<TEntity, Guid> 
    where TEntity : IEntity
{
}