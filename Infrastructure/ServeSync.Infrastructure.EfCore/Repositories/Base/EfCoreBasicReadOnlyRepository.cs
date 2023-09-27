using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Infrastructure.EfCore.Common;

namespace ServeSync.Infrastructure.EfCore.Repositories.Base;

public class EfCoreBasicReadOnlyRepository<TAggregateRoot, TKey> : EfCoreSpecificationRepository<TAggregateRoot, TKey>, IBasicReadOnlyRepository<TAggregateRoot, TKey>
    where TAggregateRoot : class, IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    public EfCoreBasicReadOnlyRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>>? expression = null)
    {
        var queryable = GetQueryable();
        return expression != null ? await queryable.Where(expression).ToListAsync() : await queryable.ToListAsync();
    }

    public async Task<IList<TAggregateRoot>> GetPagedListAsync(int skip, int take, Expression<Func<TAggregateRoot, bool>> expression, string? sorting = null, bool tracking = true, string? includeProps = null)
    {
        var queryable = new AppQueryableBuilder<TAggregateRoot, TKey>(GetQueryable(tracking))
                                    .IncludeProp(includeProps)
                                    .ApplyFilter(expression)
                                    .ApplySorting(sorting)
                                    .Build();

        return await queryable.Skip(skip).Take(take).ToListAsync();
    }

    public Task<TAggregateRoot?> FindByIdAsync(object id, string? includeProps = null, bool tracking = true)
    {
        var queryable = new AppQueryableBuilder<TAggregateRoot, TKey>(GetQueryable(tracking))
                                    .IncludeProp(includeProps)
                                    .Build();

        return queryable.FirstOrDefaultAsync(x => id.Equals(x.Id));
    }

    public Task<TAggregateRoot?> FindAsync(Expression<Func<TAggregateRoot, bool>> expression, bool tracking = true,
        string? includeProps = null)
    {
        var queryable = new AppQueryableBuilder<TAggregateRoot, TKey>(GetQueryable(tracking))
                                    .IncludeProp(includeProps)
                                    .Build();
        
        return queryable.FirstOrDefaultAsync(expression);
    }

    public Task<bool> IsExistingAsync(Guid id)
    {
        var queryable = GetQueryable();
        return queryable.AnyAsync(x => id.Equals(x.Id));
    }

    public Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>>? expression = null)
    {
        var queryable = GetQueryable();
        return expression != null ? queryable.AnyAsync(expression) : queryable.AnyAsync();
    }

    public Task<bool> AllAsync(Expression<Func<TAggregateRoot, bool>> expression)
    {
        var queryable = GetQueryable();
        return queryable.AllAsync(expression);
    }

    public Task<int> GetCountAsync(Expression<Func<TAggregateRoot, bool>>? expression = null)
    {
        var queryable = GetQueryable();
        return expression != null ? queryable.CountAsync(expression) : queryable.CountAsync();
    }

    public Task<decimal> GetAverageAsync(Expression<Func<TAggregateRoot, decimal>> selector, Expression<Func<TAggregateRoot, bool>>? expression = null)
    {
        var queryable = GetQueryable();
        if (expression != null)
        {
            queryable = queryable.Where(expression);
        }

        return queryable.AverageAsync(selector);
    }
    
    protected virtual IQueryable<TAggregateRoot> GetQueryable(bool tracking = true)
    {
        return new AppQueryableBuilder<TAggregateRoot, TKey>(DbSet, false).Build();
    }
}

public class EfCoreBasicReadOnlyRepository<TAggregateRoot> : EfCoreBasicReadOnlyRepository<TAggregateRoot, Guid>
    where TAggregateRoot : AggregateRoot<Guid>
{
    public EfCoreBasicReadOnlyRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}