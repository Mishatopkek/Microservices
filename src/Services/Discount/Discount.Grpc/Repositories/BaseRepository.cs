using Dapper;
using Npgsql;

namespace Discount.Grpc.Repositories;

public class BaseRepository
{
    private readonly string _connectionString;
    public BaseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        await using NpgsqlConnection connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        await using NpgsqlConnection connection = CreateConnection();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    protected async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        await using NpgsqlConnection connection = CreateConnection();
        return await connection.ExecuteAsync(sql, parameters);
    }

    // Other Helpers...

    private NpgsqlConnection CreateConnection()
    {
        NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        // Properly initialize your connection here.
        return connection;
    }
}