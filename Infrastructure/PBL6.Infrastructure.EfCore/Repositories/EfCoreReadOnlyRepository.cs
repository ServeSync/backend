using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PBL6.Domain.SeedWorks.Models;
using PBL6.Domain.SeedWorks.Repositories;
using PBL6.Infrastructure.EfCore.Common;

namespace PBL6.Infrastructure.EfCore.Repositories;

public class EfCoreReadOnlyRepository<TDbContext, TAggregateRoot, TKey> : EfCoreSpecificationRepository<TDbContext, TAggregateRoot, TKey>, IReadOnlyRepository<TAggregateRoot, TKey>
    where TDbContext : DbContext
    where TAggregateRoot : AggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    public EfCoreReadOnlyRepository(TDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>>? expression = null)
    {
        return expression != null ? await DbSet.Where(expression).ToListAsync() : await DbSet.ToListAsync();
    }

    public async Task<IList<TAggregateRoot>> GetPagedListAsync(int skip, int take, Expression<Func<TAggregateRoot, bool>> expression, string? sorting = null, bool tracking = true, string? includeProps = null)
    {
        var queryable = new AppQueryableBuilder<TAggregateRoot, TKey>(DbSet, tracking)
                                    .IncludeProp(includeProps)
                                    .ApplyFilter(expression)
                                    .ApplySorting(sorting)
                                    .Build();

        return await queryable.Skip(skip).Take(take).ToListAsync();
    }

    public Task<TAggregateRoot?> FindByIdAsync(object id, string? includeProps = null, bool tracking = true)
    {
        var queryable = new AppQueryableBuilder<TAggregateRoot, TKey>(DbSet, tracking)
                                    .IncludeProp(includeProps)
                                    .Build();

        return queryable.FirstOrDefaultAsync(x => id.Equals(x.Id));
    }

    public Task<TAggregateRoot?> FindAsync(Expression<Func<TAggregateRoot, bool>> expression, bool tracking = true,
        string? includeProps = null)
    {
        var queryable = new AppQueryableBuilder<TAggregateRoot, TKey>(DbSet, tracking)
                                    .IncludeProp(includeProps)
                                    .Build();

        return queryable.FirstOrDefaultAsync(expression);
    }

    public Task<bool> IsExistingAsync(Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return DbSet.AnyAsync(x => id.Equals(x.Id));
    }

    public Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>>? expression = null)
    {
        return expression != null ? DbSet.AnyAsync(expression) : DbSet.AnyAsync();
    }

    public Task<bool> AllAsync(Expression<Func<TAggregateRoot, bool>> expression)
    {
        return DbSet.AllAsync(expression);
    }

    public Task<int> GetCountAsync(Expression<Func<TAggregateRoot, bool>>? expression = null)
    {
        return expression != null ? DbSet.CountAsync(expression) : DbSet.CountAsync();
    }

    public Task<decimal> GetAverageAsync(Expression<Func<TAggregateRoot, decimal>> selector, Expression<Func<TAggregateRoot, bool>>? expression = null)
    {
        var queryable = DbSet.AsQueryable();
        if (expression != null)
        {
            queryable = queryable.Where(expression);
        }

        return queryable.AverageAsync(selector);
    }
}

public class EfCoreReadOnlyRepository<TAggregateRoot> : EfCoreReadOnlyRepository<DbContext, TAggregateRoot, Guid>
    where TAggregateRoot : AggregateRoot<Guid>
{
    public EfCoreReadOnlyRepository(DbContext dbContext) : base(dbContext)
    {
    }
}