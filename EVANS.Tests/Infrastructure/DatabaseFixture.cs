using Microsoft.Data.SqlClient;
using Respawn;
using Testcontainers.MsSql;

namespace EVANS.Tests.Infrastructure;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _evans;
    private readonly MsSqlContainer _yearly;
    private Respawner _evansRespawner = null!;
    private Respawner _yearlyRespawner = null!;

    public string EvansConnectionString => _evans.GetConnectionString();
    public string YearlyConnectionString => _yearly.GetConnectionString();

    public DatabaseFixture()
    {
        _evans  = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2022-latest").Build();
        _yearly = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2022-latest").Build();
    }

    public async Task InitializeAsync()
    {
        await Task.WhenAll(_evans.StartAsync(), _yearly.StartAsync());
        await ApplySchema(EvansConnectionString,  SchemaPath("schema-evans.sql"));
        await ApplySchema(YearlyConnectionString, SchemaPath("schema-yearly.sql"));

        _evansRespawner  = await Respawner.CreateAsync(EvansConnectionString,  new RespawnerOptions { DbAdapter = DbAdapter.SqlServer });
        _yearlyRespawner = await Respawner.CreateAsync(YearlyConnectionString, new RespawnerOptions { DbAdapter = DbAdapter.SqlServer });
    }

    public async Task DisposeAsync()
    {
        await Task.WhenAll(_evans.DisposeAsync().AsTask(), _yearly.DisposeAsync().AsTask());
    }

    public async Task ResetAsync()
    {
        await Task.WhenAll(
            _evansRespawner.ResetAsync(EvansConnectionString),
            _yearlyRespawner.ResetAsync(YearlyConnectionString));
    }

    private static async Task ApplySchema(string connectionString, string scriptPath)
    {
        var script = await File.ReadAllTextAsync(scriptPath);
        await using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();

        foreach (var batch in script.Split("\nGO", StringSplitOptions.RemoveEmptyEntries))
        {
            var sql = batch.Trim();
            if (string.IsNullOrEmpty(sql)) continue;
            await using var cmd = new SqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }
    }

    private static string SchemaPath(string file)
    {
        var dir = AppContext.BaseDirectory;
        while (dir != null && !File.Exists(Path.Combine(dir, "db", file)))
            dir = Directory.GetParent(dir)?.FullName;
        return Path.Combine(dir!, "db", file);
    }
}
