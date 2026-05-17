using Microsoft.Data.SqlClient;

namespace EVANS.Tests.Infrastructure;

[Collection("Database")]
public abstract class TestBase : IAsyncLifetime
{
    protected readonly DatabaseFixture Db;

    protected TestBase(DatabaseFixture db) => Db = db;

    public Task InitializeAsync() => Db.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    protected SqlConnection EvansConn() => new(Db.EvansConnectionString);
    protected SqlConnection YearlyConn() => new(Db.YearlyConnectionString);

    protected async Task<int> ExecScalarAsync(SqlConnection conn, string sql, Action<SqlCommand>? setup = null)
    {
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        setup?.Invoke(cmd);
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    protected async Task ExecAsync(SqlConnection conn, string sql, Action<SqlCommand>? setup = null)
    {
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        setup?.Invoke(cmd);
        await cmd.ExecuteNonQueryAsync();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }
