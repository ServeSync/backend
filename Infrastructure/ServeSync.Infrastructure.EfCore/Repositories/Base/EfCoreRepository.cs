using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Infrastructure.EfCore.Repositories.Base;

public class EfCoreRepository<TAggregateRoot, TKey> : EfCoreReadOnlyRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
    where TAggregateRoot : class, IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    public EfCoreRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public void Delete(TAggregateRoot entity)
    {
        // if (typeof(IHasSoftDeleteEntity<TKey>).IsAssignableFrom(typeof(TAggregateRoot)))
        // {
        //     ((IHasSoftDeleteEntity<TKey>)entity).Delete();
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

public class EfCoreRepository<TAggregateRoot> : EfCoreRepository<TAggregateRoot, Guid>
    where TAggregateRoot : AggregateRoot
{
    public EfCoreRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}