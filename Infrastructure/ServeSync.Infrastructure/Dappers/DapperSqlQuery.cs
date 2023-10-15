using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;

namespace ServeSync.Infrastructure.Dappers;

public class DapperSqlQuery : ISqlQuery
{
    private readonly string _connectionString;
    
    public DapperSqlQuery(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? throw new Exception("Default connection string is not provided!");
    }
    
    public async Task<IEnumerable<T>> QueryListAsync<T>(string sql, object? param = null)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryAsync<T>(sql, param);
    }
}