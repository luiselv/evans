using Dapper;
using EVANS.Domain.GuiaRemision;
using EVANS.Infrastructure.Sql.GuiaRemision;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Tests.GuiaRemision;

[Collection("GuiaRepository")]
public class RecepcionVinculadaServiceSqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public RecepcionVinculadaServiceSqlTests(GuiaRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // (a) VincularRecepcion updates RECE_GUIAREMISION to "SERIE-NUMERO" format
    // ------------------------------------------------------------------

    [Fact]
    public async Task VincularRecepcion_updates_RECE_GUIAREMISION_to_SERIE_NUMERO_format()
    {
        // Arrange — insert a Recepcion row
        using var yearlyConn = new SqlConnection(_fixture.YearlyConnectionString);
        var recepcionId = await yearlyConn.ExecuteScalarAsync<int>(@"
            INSERT INTO Recepcion (RECE_FECHAEMISION, CLIE_REMITENTE, CLIE_DESTINATARIO, ESTA_CODIGO, RECE_BULTOS)
            VALUES (GETDATE(), 1, 2, 1, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);");

        var service = new RecepcionVinculadaServiceSql(new FixtureYearlyConnectionFactoryForRecepcion(_fixture));
        var numero = new NumeroGuia("GR01", 42);

        // Act
        service.VincularRecepcion(recepcionId, numero, GuiaRepositoryFixture.TestYear);

        // Assert
        var valor = await yearlyConn.ExecuteScalarAsync<string>(
            "SELECT RECE_GUIAREMISION FROM Recepcion WHERE RECE_CODIGO = @id",
            new { id = recepcionId });

        valor.Should().Be("GR01-000042");
    }

    // ------------------------------------------------------------------
    // (b) Non-existent recepcionId does not throw (idempotent UPDATE)
    // ------------------------------------------------------------------

    [Fact]
    public void Non_existent_recepcionId_does_not_throw()
    {
        var service = new RecepcionVinculadaServiceSql(new FixtureYearlyConnectionFactoryForRecepcion(_fixture));
        var numero = new NumeroGuia("GR01", 99999);

        var ex = Record.Exception(() =>
            service.VincularRecepcion(recepcionId: 9999999, numero, GuiaRepositoryFixture.TestYear));

        ex.Should().BeNull();
    }
}

file sealed class FixtureYearlyConnectionFactoryForRecepcion(GuiaRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) =>
        new(fixture.YearlyConnectionString);

    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() =>
        new(fixture.YearlyConnectionString);
}
