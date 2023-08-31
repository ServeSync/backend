using System.Linq.Expressions;
using PBL6.Domain.SeedWorks.Models;
using PBL6.Domain.SeedWorks.Models.Interfaces;
using PBL6.Domain.SeedWorks.Specifications.Interfaces;

namespace PBL6.Domain.SeedWorks.Specifications;

public class AndSpecification<TEntity, TKey> : Specification<TEntity, TKey> 
    where TEntity : IEntity<TKey> 
    where TKey : IEquatable<TKey>
{
    private readonly ISpecification<TEntity, TKey> _left;

    private readonly ISpecification<TEntity, TKey> _right;

    public AndSpecification(ISpecification<TEntity, TKey> left, ISpecification<TEntity, TKey> right) : base(left)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var parameter = Expression.Parameter(typeof(TEntity));
        var body = Expression.AndAlso(
            Expression.Invoke(leftExpression, parameter),
            Expression.Invoke(rightExpression, parameter)
        );

        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
}

public class AndSpecification<TEntity> : AndSpecification<TEntity, Guid>
    where TEntity : IEntity
{
    public AndSpecification(ISpecification<TEntity, Guid> left, ISpecification<TEntity, Guid> right) : base(left, right)
    {
    }
}