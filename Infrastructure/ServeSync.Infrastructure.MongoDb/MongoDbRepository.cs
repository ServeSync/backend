using System.Linq.Expressions;
using MongoDB.Driver;
using ServeSync.Application.ReadModels.Abstracts;

namespace ServeSync.Infrastructure.MongoDb;

public class MongoDbRepository<T, TKey> : IReadModelRepository<T, TKey>
    where T : BaseReadModel<TKey> 
    where TKey : IEquatable<TKey>
{
    protected readonly IMongoCollection<T> Collection;
    private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    public MongoDbRepository(IMongoDatabase database, string collectionName)
    {
        Collection = database.GetCollection<T>(collectionName);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await Collection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await Collection.Find(filter).ToListAsync();
    }

    public async Task<T> GetAsync(TKey id)
    {
        var filter = _filterBuilder.Eq(e => e.Id, id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        await Collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        
        var filter = _filterBuilder.Eq(e => e.Id, entity.Id);
        await Collection.ReplaceOneAsync(filter, entity);
    }

    public Task CreateOrUpdateAsync(T entity)
    {
        var filter = _filterBuilder.Eq(e => e.Id, entity.Id);
        return Collection.FindOneAndReplaceAsync(filter, entity, new FindOneAndReplaceOptions<T, T>
        {
            IsUpsert = true
        });
    }

    public async Task RemoveAsync(TKey id)
    {
        var filter = _filterBuilder.Eq<TKey>(e => e.Id, id);
        await Collection.DeleteOneAsync(filter);
    }
}