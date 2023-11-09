using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Infrastructure.EfCore.Common;

public class AppQueryableBuilder<TEntity, TKey> 
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private IQueryable<TEntity> _queryable;

    public AppQueryableBuilder(DbSet<TEntity> dbSet, bool tracking)
    {
        _queryable = tracking ? dbSet.AsQueryable() : dbSet.AsNoTracking();
    }

    public AppQueryableBuilder(IQueryable<TEntity> queryable)
    {
        _queryable = queryable;
    }

    public AppQueryableBuilder<TEntity, TKey> ApplyFilter(Expression<Func<TEntity, bool>>? expression)
    {
        if (expression != null)
        {
            _queryable = _queryable.Where(expression);    
        }
        
        return this;
    }

    public AppQueryableBuilder<TEntity, TKey> ApplySorting(string? sorting)
    {
        if (typeof(TEntity).IsAssignableTo(typeof(IHasAuditable)))
        {
            if (string.IsNullOrWhiteSpace(sorting))
            {
                _queryable = _queryable.OrderBy(GetDefaultSorting());  
            }
            else
            {
                _queryable = _queryable.OrderBy($"{sorting}, {GetDefaultSorting()}");
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(sorting))
            {
                _queryable = _queryable.OrderBy(sorting);    
            }
        }
        return this;
    }

    public AppQueryableBuilder<TEntity, TKey> IncludeProp(Expression<Func<TEntity, object>> includeProps)
    {
        _queryable = _queryable.Include(includeProps);
        return this;
    }
    
    public AppQueryableBuilder<TEntity, TKey> IncludeProp(IEnumerable<Expression<Func<TEntity, object>>> includeProps)
    {
        foreach (var expression in includeProps)
        {
            _queryable = _queryable.Include(expression);
        }
        
        return this;
    }

    public AppQueryableBuilder<TEntity, TKey> IncludeProp(string? includeProps)
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
    
    public AppQueryableBuilder<TEntity, TKey> IncludeProp(IEnumerable<string> includeProps)
    {
        foreach (var prop in includeProps)
        {
            IncludeProp(prop);
        }
        
        return this;
    }

    public IQueryable<TEntity> Build()
    {
        return _queryable;
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