using System.Linq.Expressions;
using PBL6.Domain.SeedWorks.Models;

namespace PBL6.Domain.SeedWorks.Specifications.Interfaces;

public interface ISpecification<TEntity, TKey> 
    where TEntity : IEntity<TKey> 
    where TKey : IEquatable<TKey> 
{
    bool IsTracking { get; set; }

    List<Expression<Func<TEntity, object>>> IncludeExpressions { get; set; }

    List<string> IncludeStrings { get; set; }

    bool IsSatisfiedBy(TEntity entity);

    ISpecification<TEntity, TKey> And(ISpecification<TEntity, TKey> specification);

    Expression<Func<TEntity, bool>> ToExpression();
}

public interface ISpecification<TEntity> : ISpecification<TEntity, Guid> 
    where TEntity : IEntity
{
}