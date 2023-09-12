namespace ServeSync.Application.SeedWorks.Queries;

public interface IDataPersistent<out TEntity> : IQueryable<TEntity> where TEntity : class
{
    IQueryable<TEntity> Get();
}