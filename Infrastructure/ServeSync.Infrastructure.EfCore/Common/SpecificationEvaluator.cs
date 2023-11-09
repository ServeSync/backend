using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Infrastructure.EfCore.Common;

public static class SpecificationEvaluator<TEntity, TKey> 
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> specification)
    {
        var queryable = specification.IsTracking ? inputQuery : inputQuery.AsNoTracking();

        if (specification.IncludeExpressions.Any())
        {
            queryable = specification.IncludeExpressions.Aggregate(queryable,
                (current, include)
                    => current.Include(include));
        }

        if (specification.IncludeStrings.Any())
        {
            queryable = specification.IncludeStrings.Aggregate(queryable,
                (current, include)
                    => current.Include(include));
        }

        return queryable.Where(specification.ToExpression());
    }

    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, IPagingAndSortingSpecification<TEntity, TKey> specification)
    {
        var queryable = GetQuery(inputQuery, (ISpecification<TEntity, TKey>)specification);

        if (typeof(TEntity).IsAssignableTo(typeof(IHasAuditable)))
        {
            if (string.IsNullOrWhiteSpace(specification.BuildSorting()))
            {
                queryable = queryable.OrderBy(GetDefaultSorting());  
            }
            else
            {
                queryable = queryable.OrderBy($"{specification.BuildSorting()}, {GetDefaultSorting()}");
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(specification.BuildSorting()))
            {
                queryable = queryable.OrderBy(specification.BuildSorting());    
            }
        }

        return queryable.Skip(specification.Skip).Take(specification.Take);
    }
    
    private static string GetDefaultSorting()
    {
        if (typeof(TEntity).IsAssignableTo(typeof(IHasAuditable)))
        {
            return $"{nameof(IAuditableEntity.Created)} desc";
        }

        return string.Empty;
    }
}