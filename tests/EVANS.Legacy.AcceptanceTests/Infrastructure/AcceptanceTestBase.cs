using Dapper;
using Microsoft.Data.SqlClient;

namespace EVANS.Legacy.AcceptanceTests.Infrastructure;

[Collection("SqlFixture")]
public abstract class AcceptanceTestBase : IAsyncLifetime
{
    protected readonly SqlFixture Fixture;

    protected AcceptanceTestBase(SqlFixture fixture)
    {
        Fixture = fixture;
    }

    public Task InitializeAsync() => Fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    protected SqlConnection OpenMain() => Fixture.OpenMain();
    protected SqlConnection OpenYear() => Fixture.OpenYear();

    protected async Task<int> GetClienteIdAsync(string nombre)
    {
        using var conn = OpenMain();
        return await conn.ExecuteScalarAsync<int>(
            "SELECT CLIE_CODIGO FROM CLIENTE WHERE CLIE_NOMBRE = @nombre", new { nombre });
    }

    protected async Task<int> InsertClienteAsync(string nombre, string ruc = "20123456789")
    {
        using var conn = OpenMain();
        return await conn.ExecuteScalarAsync<int>(@"
            INSERT INTO CLIENTE (CLIE_NOMBRE, IDEN_CODIGO, CLIE_NROIDENTIFICACION)
            VALUES (@nombre, 1, @ruc);
            SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { nombre, ruc });
    }
}

[CollectionDefinition("SqlFixture")]
public class SqlFixtureCollection : ICollectionFixture<SqlFixture> { }
