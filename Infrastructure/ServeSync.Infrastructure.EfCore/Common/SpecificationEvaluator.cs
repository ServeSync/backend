using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Infrastructure.EfCore.Common;

public static class SpecificationEvaluator<TAggregateRoot, TKey> 
    where TAggregateRoot : AggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    public static IQueryable<TAggregateRoot> GetQuery(IQueryable<TAggregateRoot> inputQuery, ISpecification<TAggregateRoot, TKey> specification)
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

    public static IQueryable<TAggregateRoot> GetQuery(IQueryable<TAggregateRoot> inputQuery, IPagingAndSortingSpecification<TAggregateRoot, TKey> specification)
    {
        var queryable = GetQuery(inputQuery, (ISpecification<TAggregateRoot, TKey>)specification);

        if (!string.IsNullOrWhiteSpace(specification.Sorting))
        {
            queryable = queryable.OrderBy(specification.BuildSorting());
        }

        return queryable.Skip(specification.Skip).Take(specification.Take);
    }
}