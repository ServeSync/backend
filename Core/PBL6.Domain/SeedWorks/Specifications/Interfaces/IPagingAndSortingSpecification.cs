using PBL6.Domain.SeedWorks.Models;
using PBL6.Domain.SeedWorks.Models.Interfaces;

namespace PBL6.Domain.SeedWorks.Specifications.Interfaces;

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