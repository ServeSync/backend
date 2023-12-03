using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Repositories;

public interface IProjection<TEntity, TKey, TOut> where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>
{
    Expression<Func<TEntity, TOut>> GetProject();
}