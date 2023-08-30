using Microsoft.EntityFrameworkCore;
using PBL6.Domain.SeedWorks.Models;
using PBL6.Domain.SeedWorks.Repositories;

namespace PBL6.Infrastructure.EfCore.Repositories;

public class EfCoreRepository<TDbContext, TAggregateRoot, TKey> : EfCoreReadOnlyRepository<TDbContext, TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
    where TDbContext : DbContext
    where TAggregateRoot : AggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    public EfCoreRepository(TDbContext dbContext) : base(dbContext)
    {
    }

    public void Delete(TAggregateRoot entity)
    {
        // if (typeof(ISoftDeleteEntity<TKey>).IsAssignableFrom(typeof(TAggregateRoot)))
        // {
        //     ((ISoftDeleteEntity<TKey>)entity).IsDeleted = true;
        //     DbSet.Update(entity);
        // }
        // else
        // {
        //     DbSet.Remove(entity);
        // }
        DbSet.Remove(entity);
    }

    public virtual async Task InsertAsync(TAggregateRoot entity)
    {
        // if (typeof(IAuditEntity<TKey>).IsAssignableFrom(typeof(TAggregateRoot)))
        // {
        //     ((IAuditEntity<TKey>)entity).CreatedDate = DateTime.UtcNow;
        // }

        await DbSet.AddAsync(entity);
    }

    public virtual void Update(TAggregateRoot entity)
    {
        // if (typeof(IAuditEntity<TKey>).IsAssignableFrom(typeof(TAggregateRoot)))
        // {
        //     ((IAuditEntity<TKey>)entity).UpdatedDate = DateTime.UtcNow;
        // }

        DbSet.Update(entity);
    }

    public virtual void Insert(TAggregateRoot entity)
    {
        // if (typeof(IAuditEntity<TKey>).IsAssignableFrom(typeof(TAggregateRoot)))
        // {
        //     ((IAuditEntity<TKey>)entity).CreatedDate = DateTime.UtcNow;
        // }

        DbSet.Add(entity);
    }

    public virtual Task InsertRangeAsync(IEnumerable<TAggregateRoot> entities)
    {
        // foreach (var entity in entities)
        // {
        //     if (typeof(IAuditEntity<TKey>).IsAssignableFrom(typeof(TAggregateRoot)))
        //     {
        //         ((IAuditEntity<TKey>)entity).CreatedDate = DateTime.UtcNow;
        //     }
        // }

        return DbSet.AddRangeAsync(entities);
    }
}

public class EfCoreRepository<TDbContext, TAggregateRoot> : EfCoreRepository<TDbContext, TAggregateRoot, Guid>
    where TDbContext : DbContext
    where TAggregateRoot : AggregateRoot
{
    public EfCoreRepository(TDbContext dbContext) : base(dbContext)
    {
    }
}