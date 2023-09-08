using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Infrastructure.EfCore.Common;

namespace ServeSync.Infrastructure.EfCore.Repositories.Base;

public class EfCoreSpecificationRepository<TDbContext, TAggregateRoot, TKey> : ISpecificationRepository<TAggregateRoot, TKey>
    where TDbContext : DbContext
    where TAggregateRoot : AggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly TDbContext DbContext;
    protected readonly DbSet<TAggregateRoot> DbSet;

    protected readonly List<Expression<Func<TAggregateRoot, object>>> DefaultIncludeExpressions;
    protected readonly List<string> DefaultIncludeStrings;
    
    public EfCoreSpecificationRepository(TDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TAggregateRoot>();

        DefaultIncludeExpressions = new();
        DefaultIncludeStrings = new();
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
    
    protected void AddInclude(Expression<Func<TAggregateRoot, object>> includeExpression)
    {
        ArgumentNullException.ThrowIfNull(includeExpression);
        
        DefaultIncludeExpressions.Add(includeExpression);
    }
    
    protected void AddInclude(string includeProp)
    {
        ArgumentException.ThrowIfNullOrEmpty(includeProp);
        
        DefaultIncludeStrings.Add(includeProp);
    }
    
    private IQueryable<TAggregateRoot> GetQueryable(ISpecification<TAggregateRoot, TKey> specification)
    {
        var specificationQueryable = SpecificationEvaluator<TAggregateRoot, TKey>.GetQuery(DbSet.AsQueryable(), specification);
        return new AppQueryableBuilder<TAggregateRoot, TKey>(specificationQueryable)
                    .IncludeProp(DefaultIncludeStrings)
                    .IncludeProp(DefaultIncludeExpressions)
                    .Build();
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