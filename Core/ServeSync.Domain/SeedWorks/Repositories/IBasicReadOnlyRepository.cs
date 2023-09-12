using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Repositories;

public interface IBasicReadOnlyRepository<TAggregateRoot, TKey> : ISpecificationRepository<TAggregateRoot, TKey> 
    where TAggregateRoot : IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    Task<IList<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>>? expression = null);

    Task<IList<TAggregateRoot>> GetPagedListAsync(int skip, int take, Expression<Func<TAggregateRoot, bool>> expression, string? sorting = null, bool tracking = true, string? includeProps = null);

    Task<TAggregateRoot?> FindByIdAsync(object id, string? includeProps = null, bool tracking = true);

    Task<TAggregateRoot?> FindAsync(Expression<Func<TAggregateRoot, bool>> expression, bool tracking = true, string? includeProps = null);

    Task<bool> IsExistingAsync(Guid id);

    Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>>? expression = null);
    
    Task<bool> AllAsync(Expression<Func<TAggregateRoot, bool>> expression);

    Task<int> GetCountAsync(Expression<Func<TAggregateRoot, bool>>? expression = null);

    Task<decimal> GetAverageAsync(Expression<Func<TAggregateRoot, decimal>> selector, Expression<Func<TAggregateRoot, bool>>? expression = null);
}