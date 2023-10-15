namespace ServeSync.Application.SeedWorks.Data;

public interface ISqlQuery
{
    Task<IEnumerable<T>> QueryListAsync<T>(string sql, object? param = null);
}