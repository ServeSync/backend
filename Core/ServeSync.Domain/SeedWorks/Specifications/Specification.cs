using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Domain.SeedWorks.Specifications;

public abstract class Specification<TEntity, TKey> : ISpecification<TEntity, TKey> 
    where TEntity : IEntity<TKey> 
    where TKey : IEquatable<TKey> 
{
    public bool IsTracking { get; set; }
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; set; }
    public List<string> IncludeStrings { get; set; }

    protected Specification(bool isTracking = false)
    {
        IsTracking = isTracking;
        IncludeExpressions = new List<Expression<Func<TEntity, object>>>();
        IncludeStrings = new List<string>();
    }

    protected Specification(ISpecification<TEntity, TKey> specification)
    {
        IsTracking = specification.IsTracking;
        IncludeExpressions = specification.IncludeExpressions;
        IncludeStrings = specification.IncludeStrings;
    }

    public bool IsSatisfiedBy(TEntity entity)
    {
        return ToExpression().Compile().Invoke(entity);
    }
    
    public ISpecification<TEntity, TKey> And(ISpecification<TEntity, TKey> specification)
    {
        return new AndSpecification<TEntity, TKey>(this, specification);
    }

    public void AddInclude(Expression<Func<TEntity, object>> expression)
    {
        IncludeExpressions.Add(expression);
    }

    public void AddInclude(string prop)
    {
        IncludeStrings.Add(prop);
    }

    public abstract Expression<Func<TEntity, bool>> ToExpression();
}

public abstract class Specification<TEntity> : Specification<TEntity, Guid>
    where TEntity : IEntity
{
}