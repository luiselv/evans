using Dapper;
using EVANS.Domain.Manifiesto;
using EVANS.Infrastructure.Sql.Manifiesto;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;

namespace EVANS.Infrastructure.Tests.Manifiesto;

[Collection("ManifiestoRepository")]
public class GuiasManifiestoServiceSqlTests : IAsyncLifetime
{
    private readonly ManifiestoRepositoryFixture _fixture;

    public GuiasManifiestoServiceSqlTests(ManifiestoRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // I-06: guias_service_marca_enviadas_actualiza_columnas_correctas
    // Verifies correct column names: EMPR_CODIGO, CHOF_CODIGO, VEHI_CODIGO, CARR_CODIGO
    // ------------------------------------------------------------------

    [Fact]
    public async Task MarcarGuiasEnviadasAsync_ActualizaColumnasCorrectas()
    {
        var service = BuildService();
        var carrier = new CarrierInfo(
            TransportistaCodigo: 1,
            ChoferCodigo: 1,
            VehiculoCodigo: 1,
            CarretaCodigo: 1);

        var result = await service.MarcarGuiasEnviadasAsync(
            guiaIds: [1],
            numero: "2024-1",
            fechaTraslado: new DateTime(2024, 6, 15),
            carrier: carrier,
            year: ManifiestoRepositoryFixture.TestYear,
            ct: CancellationToken.None);

        result.Affected.Should().Be(1);
        result.NotFound.Should().BeEmpty();

        // Verify actual DB values
        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var row = await conn.QuerySingleAsync(
            @"SELECT GREM_ENVIADO, GREM_MANIFIESTO, GREM_NROMANIFIESTO,
                     GREM_FECHATRASLADO, EMPR_CODIGO, CHOF_CODIGO, VEHI_CODIGO, CARR_CODIGO
              FROM GuiaRemision WHERE GREM_CODIGO = 1");

        ((int)row.GREM_ENVIADO).Should().Be(1);
        ((int)row.GREM_MANIFIESTO).Should().Be(1);
        ((string)row.GREM_NROMANIFIESTO).Should().Be("2024-1");
        ((int)row.EMPR_CODIGO).Should().Be(1);
        ((int)row.CHOF_CODIGO).Should().Be(1);
        ((int)row.VEHI_CODIGO).Should().Be(1);
        ((int)row.CARR_CODIGO).Should().Be(1);
    }

    // ------------------------------------------------------------------
    // I-06b: CARR_CODIGO nullable — sets NULL when CarretaCodigo is null
    // ------------------------------------------------------------------

    [Fact]
    public async Task MarcarGuiasEnviadasAsync_CarretaNula_SetNullEnColumna()
    {
        var service = BuildService();
        var carrier = new CarrierInfo(
            TransportistaCodigo: 1,
            ChoferCodigo: 1,
            VehiculoCodigo: 1,
            CarretaCodigo: null);  // no carreta

        await service.MarcarGuiasEnviadasAsync(
            guiaIds: [1],
            numero: "2024-1",
            fechaTraslado: new DateTime(2024, 6, 15),
            carrier: carrier,
            year: ManifiestoRepositoryFixture.TestYear,
            ct: CancellationToken.None);

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var carretaCodigo = await conn.ExecuteScalarAsync<int?>(
            "SELECT CARR_CODIGO FROM GuiaRemision WHERE GREM_CODIGO = 1");

        carretaCodigo.Should().BeNull();
    }

    // ------------------------------------------------------------------
    // I-07: guias_service_marca_disponibles_resetea_flags
    // GREM_ENVIADO=0, GREM_NROMANIFIESTO='', GREM_FECHATRASLADO=GREM_FECHAEMISION
    // GREM_MANIFIESTO is NOT reset (legacy SC-4)
    // ------------------------------------------------------------------

    [Fact]
    public async Task MarcarGuiasDisponiblesAsync_ResetearFlags()
    {
        var service = BuildService();

        // First mark as sent
        var carrier = new CarrierInfo(1, 1, 1, 1);
        await service.MarcarGuiasEnviadasAsync(
            guiaIds: [1],
            numero: "2024-1",
            fechaTraslado: new DateTime(2024, 6, 15),
            carrier: carrier,
            year: ManifiestoRepositoryFixture.TestYear,
            ct: CancellationToken.None);

        // Then mark as available
        var result = await service.MarcarGuiasDisponiblesAsync(
            guiaIds: [1],
            year: ManifiestoRepositoryFixture.TestYear,
            ct: CancellationToken.None);

        result.Affected.Should().Be(1);
        result.NotFound.Should().BeEmpty();

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var row = await conn.QuerySingleAsync(
            @"SELECT GREM_ENVIADO, GREM_NROMANIFIESTO, GREM_FECHATRASLADO, GREM_FECHAEMISION, GREM_MANIFIESTO
              FROM GuiaRemision WHERE GREM_CODIGO = 1");

        ((int)row.GREM_ENVIADO).Should().Be(0);
        ((string)row.GREM_NROMANIFIESTO).Should().BeEmpty();
        // GREM_FECHATRASLADO should be reset to GREM_FECHAEMISION
        ((DateTime)row.GREM_FECHATRASLADO).Should().Be((DateTime)row.GREM_FECHAEMISION);
        // GREM_MANIFIESTO is NOT reset (legacy SC-4 behavior)
        ((int)row.GREM_MANIFIESTO).Should().Be(1);
    }

    // ------------------------------------------------------------------
    // I-08: guias_service_not_found_reportado_en_result
    // Non-existent GuiaId appears in NotFound; Affected=0
    // ------------------------------------------------------------------

    [Fact]
    public async Task MarcarGuiasEnviadasAsync_GuiaNoExiste_ReportadaEnNotFound()
    {
        var service = BuildService();
        var carrier = new CarrierInfo(1, 1, 1, null);

        const int nonExistentGuiaId = 999999;

        var result = await service.MarcarGuiasEnviadasAsync(
            guiaIds: [nonExistentGuiaId],
            numero: "2024-1",
            fechaTraslado: new DateTime(2024, 6, 15),
            carrier: carrier,
            year: ManifiestoRepositoryFixture.TestYear,
            ct: CancellationToken.None);

        result.Affected.Should().Be(0);
        result.NotFound.Should().Contain(nonExistentGuiaId);
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private GuiasManifiestoServiceSql BuildService() =>
        new(new FixtureYearlyConnectionFactoryForGuias(_fixture),
            NullLogger<GuiasManifiestoServiceSql>.Instance);
}

file sealed class FixtureYearlyConnectionFactoryForGuias(ManifiestoRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) =>
        new(fixture.YearlyConnectionString);

    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() =>
        new(fixture.YearlyConnectionString);
}
