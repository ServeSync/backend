using Microsoft.EntityFrameworkCore;
using PBL6.Domain.SeedWorks.Models;
using PBL6.Domain.SeedWorks.Repositories;
using PBL6.Domain.SeedWorks.Specifications.Interfaces;
using PBL6.Infrastructure.EfCore.Common;

namespace PBL6.Infrastructure.EfCore.Repositories;

public class EfCoreSpecificationRepository<TDbContext, TAggregateRoot, TKey> : ISpecificationRepository<TAggregateRoot, TKey>
    where TDbContext : DbContext
    where TAggregateRoot : AggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly TDbContext DbContext;
    protected readonly DbSet<TAggregateRoot> DbSet;
    
    public EfCoreSpecificationRepository(TDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TAggregateRoot>();
    }
    public async Task<IList<TAggregateRoot>> GetPagedListAsync(int skip, int take, ISpecification<TAggregateRoot, TKey> specification, string? sorting = null)
    {
        return await SpecificationEvaluator<TAggregateRoot, TKey>.GetQuery(DbSet.AsQueryable(), specification)
            .Skip(skip).Take(take).ToListAsync();
    }

    public async Task<IList<TAggregateRoot>> GetPagedListAsync(IPagingAndSortingSpecification<TAggregateRoot, TKey> specification)
    {
        return await SpecificationEvaluator<TAggregateRoot, TKey>.GetQuery(DbSet.AsQueryable(), specification).ToListAsync();
    }

    public Task<TAggregateRoot?> FindAsync(ISpecification<TAggregateRoot, TKey> specification)
    {
        return SpecificationEvaluator<TAggregateRoot, TKey>.GetQuery(DbSet.AsQueryable(), specification).FirstOrDefaultAsync();
    }

    public Task<int> GetCountAsync(ISpecification<TAggregateRoot, TKey> specification)
    {
        return DbSet.CountAsync(specification.ToExpression());
    }

    public Task<bool> AnyAsync(ISpecification<TAggregateRoot, TKey> specification)
    {
        return DbSet.AnyAsync(specification.ToExpression());
    }

    public Task<bool> AllAsync(ISpecification<TAggregateRoot, TKey> specification)
    {
        return DbSet.AllAsync(specification.ToExpression());
    }
}

public class EfCoreSpecificationRepository<TDbContext, TAggregateRoot> : EfCoreSpecificationRepository<TDbContext, TAggregateRoot, Guid>
    where TDbContext : DbContext
    where TAggregateRoot : AggregateRoot
{
    public EfCoreSpecificationRepository(TDbContext dbContext) : base(dbContext)
    {
    }
}