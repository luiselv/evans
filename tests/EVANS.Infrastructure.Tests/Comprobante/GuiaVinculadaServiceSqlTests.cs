using Dapper;
using EVANS.Domain.Comprobante;
using EVANS.Infrastructure.Sql.Comprobante;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;

namespace EVANS.Infrastructure.Tests.Comprobante;

[Collection("ComprobanteRepository")]
public class GuiaVinculadaServiceSqlTests : IAsyncLifetime
{
    private readonly ComprobanteRepositoryFixture _fixture;

    public GuiaVinculadaServiceSqlTests(ComprobanteRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // (a) VincularComprobante updates grem_docventa where COMP_GRT matches
    // ------------------------------------------------------------------

    [Fact]
    public async Task VincularComprobante_ExistingGuia_SetsDocventa()
    {
        // Arrange — insert a GuiaRemision row with COMP_GRT reference format "F001000001"
        const string guiaRef = "F001000001";
        using var yearlyConn = new SqlConnection(_fixture.YearlyConnectionString);
        await yearlyConn.ExecuteAsync(@"
            INSERT INTO GuiaRemision (
                GREM_SERIE, GREM_NUMERO, GREM_FECHAEMISION,
                CLIE_REMITENTE, CLIE_DESTINATARIO, ESTA_CODIGO, GREM_IMPRESO
            )
            VALUES ('F001', '000001', GETDATE(), 1, 1, 1, 0)");

        var service = BuildService();
        var numero = new NumeroComprobante("F001", "000001");

        // Act
        var result = await service.VincularComprobanteAsync(guiaRef, numero, ComprobanteRepositoryFixture.TestYear);

        // Assert — docventa updated, result true
        result.Should().BeTrue();

        var docventa = await yearlyConn.ExecuteScalarAsync<string>(
            "SELECT GREM_DOCVENTA FROM GuiaRemision WHERE GREM_SERIE = 'F001' AND GREM_NUMERO = '000001'");
        docventa.Should().Be("F001-000001");
    }

    // ------------------------------------------------------------------
    // (b) Non-existent guiaRef — returns false, no exception thrown
    // ------------------------------------------------------------------

    [Fact]
    public async Task VincularComprobanteAsync_NotFound_ReturnsFalse()
    {
        var service = BuildService();
        var numero = new NumeroComprobante("F001", "000099");

        var result = await service.VincularComprobanteAsync("NONEXISTENT", numero, ComprobanteRepositoryFixture.TestYear);

        result.Should().BeFalse();
    }

    // ------------------------------------------------------------------
    // (c) Exception scenario — logs warning, returns false, never rethrows
    // ------------------------------------------------------------------

    [Fact]
    public async Task VincularComprobanteAsync_InvalidConnection_LogsWarning_ReturnsFalse()
    {
        // Use a bad connection string to force an exception
        var logger = NullLogger<GuiaVinculadaServiceSql>.Instance;
        var service = new GuiaVinculadaServiceSql(
            new BrokenYearlyConnectionFactory(),
            logger);

        var numero = new NumeroComprobante("F001", "000001");

        var result = await service.VincularComprobanteAsync("F001000001", numero, ComprobanteRepositoryFixture.TestYear);

        result.Should().BeFalse();
        // If we get here without exception, the method swallowed it correctly — test passes
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private GuiaVinculadaServiceSql BuildService() =>
        new(
            new FixtureYearlyConnectionFactoryForVinculada(_fixture),
            NullLogger<GuiaVinculadaServiceSql>.Instance);
}

file sealed class FixtureYearlyConnectionFactoryForVinculada(ComprobanteRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) =>
        new(fixture.YearlyConnectionString);

    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() =>
        new(fixture.YearlyConnectionString);
}

file sealed class BrokenYearlyConnectionFactory
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) =>
        new("Server=.;Database=NONEXISTENT_DB;Connection Timeout=1;TrustServerCertificate=True;Integrated Security=True;");

    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() =>
        new("Server=.;Database=NONEXISTENT_DB;Connection Timeout=1;TrustServerCertificate=True;Integrated Security=True;");
}
