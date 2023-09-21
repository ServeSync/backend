using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Infrastructure.EfCore.Common;

namespace ServeSync.Infrastructure.EfCore.Repositories.Base;

public class EfCoreSpecificationRepository<TAggregateRoot, TKey> : ISpecificationRepository<TAggregateRoot, TKey>
    where TAggregateRoot : class, IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<TAggregateRoot> DbSet;
    
    public EfCoreSpecificationRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TAggregateRoot>();
    }

    public async Task<IList<TAggregateRoot>> FilterAsync(ISpecification<TAggregateRoot, TKey> specification)
    {
        return await GetQueryable(specification).ToListAsync();
    }

    public async Task<IList<TAggregateRoot>> GetPagedListAsync(int skip, int take, ISpecification<TAggregateRoot, TKey> specification, string? sorting = null)
    {
        return await GetQueryable(specification)
            .Skip(skip).Take(take).ToListAsync();
    }

    public async Task<IList<TAggregateRoot>> GetPagedListAsync(IPagingAndSortingSpecification<TAggregateRoot, TKey> specification)
    {
        return await GetQueryable(specification).ToListAsync();
    }

    public Task<TAggregateRoot?> FindAsync(ISpecification<TAggregateRoot, TKey> specification)
    {
        return GetQueryable(specification).FirstOrDefaultAsync();
    }

    public Task<int> GetCountAsync(ISpecification<TAggregateRoot, TKey> specification)
    {
        return GetQueryable(specification).CountAsync(specification.ToExpression());
    }

    public Task<bool> AnyAsync(ISpecification<TAggregateRoot, TKey> specification)
    {
        return GetQueryable(specification).AnyAsync(specification.ToExpression());
    }

    public Task<bool> AllAsync(ISpecification<TAggregateRoot, TKey> specification)
    {
        return GetQueryable(specification).AllAsync(specification.ToExpression());
    }
    
    protected virtual IQueryable<TAggregateRoot> GetQueryable(ISpecification<TAggregateRoot, TKey> specification)
    {
        return SpecificationEvaluator<TAggregateRoot, TKey>.GetQuery(DbSet.AsQueryable(), specification);
    }
    
    protected virtual IQueryable<TAggregateRoot> GetQueryable(IPagingAndSortingSpecification<TAggregateRoot, TKey> specification)
    {
        return SpecificationEvaluator<TAggregateRoot, TKey>.GetQuery(DbSet.AsQueryable(), specification);
    }
}

public class EfCoreSpecificationRepository<TAggregateRoot> : EfCoreSpecificationRepository<TAggregateRoot, Guid>
    where TAggregateRoot : AggregateRoot
{
    public EfCoreSpecificationRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}