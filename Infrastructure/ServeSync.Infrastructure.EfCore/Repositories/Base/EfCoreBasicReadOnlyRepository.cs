using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.EfCore.Common;

namespace ServeSync.Infrastructure.EfCore.Repositories.Base;

public class EfCoreBasicReadOnlyRepository<TEntity, TKey> : EfCoreSpecificationRepository<TEntity, TKey>, IBasicReadOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public EfCoreBasicReadOnlyRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<TEntity>> FindByIncludedIdsAsync(IEnumerable<TKey> ids)
    {
        return await GetQueryable().Where(x => ids.Contains(x.Id)).ToListAsync();
    }

    public async Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? expression = null, string? sorting = null)
    {
        return await new AppQueryableBuilder<TEntity, TKey>(GetQueryable())
                                    .ApplyFilter(expression)
                                    .ApplySorting(sorting)
                                    .Build()
                                    .ToListAsync();
    }

    public async Task<IList<TEntity>> GetPagedListAsync(int skip, int take, Expression<Func<TEntity, bool>> expression, string? sorting = null, bool tracking = true, string? includeProps = null)
    {
        var queryable = new AppQueryableBuilder<TEntity, TKey>(GetQueryable(tracking))
                                    .IncludeProp(includeProps)
                                    .ApplyFilter(expression)
                                    .ApplySorting(sorting)
                                    .Build();

        return await queryable.Skip(skip).Take(take).ToListAsync();
    }

    public Task<TEntity?> FindByIdAsync(object id, string? includeProps = null, bool tracking = true)
    {
        var queryable = new AppQueryableBuilder<TEntity, TKey>(GetQueryable(tracking))
                                    .IncludeProp(includeProps)
                                    .Build();

        return queryable.FirstOrDefaultAsync(x => id.Equals(x.Id));
    }

    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true,
        string? includeProps = null)
    {
        var queryable = new AppQueryableBuilder<TEntity, TKey>(GetQueryable(tracking))
                                    .IncludeProp(includeProps)
                                    .Build();
        
        return queryable.FirstOrDefaultAsync(expression);
    }

    public Task<bool> IsExistingAsync(Guid id)
    {
        var queryable = GetQueryable();
        return queryable.AnyAsync(x => id.Equals(x.Id));
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        var queryable = GetQueryable();
        return expression != null ? queryable.AnyAsync(expression) : queryable.AnyAsync();
    }

    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> expression)
    {
        var queryable = GetQueryable();
        return queryable.AllAsync(expression);
    }

    public Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        var queryable = GetQueryable();
        return expression != null ? queryable.CountAsync(expression) : queryable.CountAsync();
    }

    public Task<decimal> GetAverageAsync(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>>? expression = null)
    {
        var queryable = GetQueryable();
        if (expression != null)
        {
            queryable = queryable.Where(expression);
        }

        return queryable.AverageAsync(selector);
    }
    
    protected virtual IQueryable<TEntity> GetQueryable(bool tracking = true)
    {
        return new AppQueryableBuilder<TEntity, TKey>(DbSet, false).Build();
    }
}

public class EfCoreBasicReadOnlyRepository<TEntity> : EfCoreBasicReadOnlyRepository<TEntity, Guid>
    where TEntity : class, IEntity<Guid>
{
    public EfCoreBasicReadOnlyRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}