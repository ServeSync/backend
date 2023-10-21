using System.Linq.Expressions;

namespace ServeSync.Application.ReadModels.Abstracts;

public interface IReadModelRepository<T, in TKey> 
    where T : BaseReadModel<TKey> 
    where TKey : IEquatable<TKey>
{
    Task CreateAsync(T entity);
    
    Task<IReadOnlyCollection<T>> GetAllAsync();
    
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    
    Task<T> GetAsync(TKey id);
    
    Task<T> GetAsync(Expression<Func<T, bool>> filter);
    
    Task RemoveAsync(TKey id);
    
    Task UpdateAsync(T entity);
    
    Task CreateOrUpdateAsync(T entity);
}