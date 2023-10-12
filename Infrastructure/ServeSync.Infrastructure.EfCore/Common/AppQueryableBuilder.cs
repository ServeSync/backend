using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Infrastructure.EfCore.Common;

public class AppQueryableBuilder<TAggregateRoot, TKey> 
    where TAggregateRoot : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private IQueryable<TAggregateRoot> _queryable;

    public AppQueryableBuilder(DbSet<TAggregateRoot> dbSet, bool tracking)
    {
        _queryable = tracking ? dbSet.AsQueryable() : dbSet.AsNoTracking();
    }

    public AppQueryableBuilder(IQueryable<TAggregateRoot> queryable)
    {
        _queryable = queryable;
    }

    public AppQueryableBuilder<TAggregateRoot, TKey> ApplyFilter(Expression<Func<TAggregateRoot, bool>> expression)
    {
        _queryable = _queryable.Where(expression);
        return this;
    }

    public AppQueryableBuilder<TAggregateRoot, TKey> ApplySorting(string? sorting)
    {
        if (!string.IsNullOrWhiteSpace(sorting))
        {
            _queryable = _queryable.OrderBy(sorting);
        }
        return this;
    }

    public AppQueryableBuilder<TAggregateRoot, TKey> IncludeProp(Expression<Func<TAggregateRoot, object>> includeProps)
    {
        _queryable = _queryable.Include(includeProps);
        return this;
    }
    
    public AppQueryableBuilder<TAggregateRoot, TKey> IncludeProp(IEnumerable<Expression<Func<TAggregateRoot, object>>> includeProps)
    {
        foreach (var expression in includeProps)
        {
            _queryable = _queryable.Include(expression);
        }
        
        return this;
    }

    public AppQueryableBuilder<TAggregateRoot, TKey> IncludeProp(string? includeProps)
    {
        if (!string.IsNullOrWhiteSpace(includeProps))
        {
            foreach (var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                _queryable = _queryable.Include(prop);
            }
        }

        return this;
    }
    
    public AppQueryableBuilder<TAggregateRoot, TKey> IncludeProp(IEnumerable<string> includeProps)
    {
        foreach (var prop in includeProps)
        {
            IncludeProp(prop);
        }
        
        return this;
    }

    public IQueryable<TAggregateRoot> Build()
    {
        return _queryable;
    }
}