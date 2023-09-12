using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Infrastructure.EfCore.Common;

namespace ServeSync.Infrastructure.EfCore.Repositories.Base;

public class EfCoreReadOnlyRepository<TAggregateRoot, TKey> : EfCoreBasicReadOnlyRepository<TAggregateRoot, TKey>, IReadOnlyRepository<TAggregateRoot, TKey>
    where TAggregateRoot : class, IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly List<Expression<Func<TAggregateRoot, object>>> _defaultIncludeExpressions;
    private readonly List<string> _defaultIncludeStrings;
    
    public EfCoreReadOnlyRepository(AppDbContext dbContext) : base(dbContext)
    {
        _defaultIncludeExpressions = new();
        _defaultIncludeStrings = new();
    }
    
    protected void AddInclude(Expression<Func<TAggregateRoot, object>> includeExpression)
    {
        ArgumentNullException.ThrowIfNull(includeExpression);
        
        _defaultIncludeExpressions.Add(includeExpression);
    }
    
    protected void AddInclude(string includeProp)
    {
        ArgumentException.ThrowIfNullOrEmpty(includeProp);
        
        _defaultIncludeStrings.Add(includeProp);
    }

    protected override IQueryable<TAggregateRoot> GetQueryable(bool tracking = true)
    {
        return new AppQueryableBuilder<TAggregateRoot, TKey>(DbSet, tracking)
            .IncludeProp(_defaultIncludeExpressions)
            .IncludeProp(_defaultIncludeStrings)
            .Build();
    }

    protected override IQueryable<TAggregateRoot> GetQueryable(ISpecification<TAggregateRoot, TKey> specification)
    {
        var specificationQueryable = SpecificationEvaluator<TAggregateRoot, TKey>.GetQuery(DbSet.AsQueryable(), specification);
        return new AppQueryableBuilder<TAggregateRoot, TKey>(specificationQueryable)
            .IncludeProp(_defaultIncludeStrings)
            .IncludeProp(_defaultIncludeExpressions)
            .Build();
    }
}

public class EfCoreReadOnlyRepository<TAggregateRoot> : EfCoreReadOnlyRepository<TAggregateRoot, Guid>
    where TAggregateRoot : AggregateRoot<Guid>
{
    public EfCoreReadOnlyRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}