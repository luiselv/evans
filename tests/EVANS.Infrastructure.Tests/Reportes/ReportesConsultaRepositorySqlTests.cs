using Dapper;
using EVANS.Application.Reportes.DTOs;
using EVANS.Infrastructure.Sql.Reportes;
using EVANS.Infrastructure.Tests.Manifiesto;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Tests.Reportes;

[Collection("ManifiestoRepository")]
public sealed class ReportesConsultaRepositorySqlTests : IAsyncLifetime
{
    private readonly ManifiestoRepositoryFixture _fixture;

    public ReportesConsultaRepositorySqlTests(ManifiestoRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ConsultarEnviosMensuales_FiltersByDateDestinationAndActiveStatus()
    {
        await SeedClientesAsync();
        await SeedGuiasAsync();

        var repository = new ReportesConsultaRepositorySql(
            new FixtureYearlyConnectionFactory(_fixture),
            new FixtureMasterConnectionFactory(_fixture));

        var filtro = new EnviosMensualesFiltro(
            Year: ManifiestoRepositoryFixture.TestYear,
            FechaDesde: new DateTime(2024, 6, 1),
            FechaHasta: new DateTime(2024, 6, 30),
            DestinoCodigos: [1]);

        var result = await repository.ConsultarEnviosMensualesAsync(
            filtro,
            ManifiestoRepositoryFixture.TestYear,
            CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Cliente.Should().Be("Cliente Uno");
        result[0].NroGuias.Should().Be(3);
        result[0].UltimoEnvio.Should().Be(new DateTime(2024, 6, 20));
    }

    [Fact]
    public async Task ConsultarEnviosMensuales_WithoutDestinations_ReturnsEmpty()
    {
        var repository = new ReportesConsultaRepositorySql(
            new FixtureYearlyConnectionFactory(_fixture),
            new FixtureMasterConnectionFactory(_fixture));

        var filtro = new EnviosMensualesFiltro(
            Year: ManifiestoRepositoryFixture.TestYear,
            FechaDesde: new DateTime(2024, 6, 1),
            FechaHasta: new DateTime(2024, 6, 30),
            DestinoCodigos: []);

        var result = await repository.ConsultarEnviosMensualesAsync(
            filtro,
            ManifiestoRepositoryFixture.TestYear,
            CancellationToken.None);

        result.Should().BeEmpty();
    }

    private async Task SeedClientesAsync()
    {
        using var conn = new SqlConnection(_fixture.MasterConnectionString);
        await conn.OpenAsync();

        await conn.ExecuteAsync(@"
            SET IDENTITY_INSERT CLIENTE ON;
            IF NOT EXISTS (SELECT 1 FROM CLIENTE WHERE CLIE_CODIGO = 1)
                INSERT INTO CLIENTE (CLIE_CODIGO, CLIE_NOMBRE, IDEN_CODIGO, CLIE_NROIDENTIFICACION)
                VALUES (1, 'Cliente Uno', 1, '20111111111');
            IF NOT EXISTS (SELECT 1 FROM CLIENTE WHERE CLIE_CODIGO = 2)
                INSERT INTO CLIENTE (CLIE_CODIGO, CLIE_NOMBRE, IDEN_CODIGO, CLIE_NROIDENTIFICACION)
                VALUES (2, 'Cliente Dos', 1, '20222222222');
            SET IDENTITY_INSERT CLIENTE OFF;");
    }

    private async Task SeedGuiasAsync()
    {
        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        await conn.OpenAsync();

        await conn.ExecuteAsync(@"
            SET IDENTITY_INSERT GuiaRemision ON;

            INSERT INTO GuiaRemision (
                GREM_CODIGO, GREM_SERIE, GREM_NUMERO,
                GREM_FECHAEMISION, GREM_FECHATRASLADO,
                CLIE_REMITENTE, CLIE_DESTINATARIO,
                DEST_CODIGO, VEHI_CODIGO, CARR_CODIGO, CHOF_CODIGO, EMPR_CODIGO,
                ESTA_CODIGO, GREM_COSTOTOTAL, GREM_PESOTOTAL,
                GREM_ENVIADO, GREM_MANIFIESTO, GREM_NROMANIFIESTO
            )
            VALUES
                (10, 'GR01', '000010', '2024-06-10', '2024-06-11', 1, 1, 1, 1, 1, 1, 1, 1, 100.0, 50.0, 0, 0, ''),
                (11, 'GR01', '000011', '2024-06-20', '2024-06-21', 1, 1, 1, 1, 1, 1, 1, 1, 110.0, 60.0, 0, 0, ''),
                (12, 'GR01', '000012', '2024-07-01', '2024-07-02', 1, 1, 1, 1, 1, 1, 1, 1, 120.0, 70.0, 0, 0, ''),
                (13, 'GR01', '000013', '2024-06-15', '2024-06-16', 1, 2, 2, 1, 1, 1, 1, 1, 130.0, 80.0, 0, 0, ''),
                (14, 'GR01', '000014', '2024-06-18', '2024-06-19', 1, 2, 1, 1, 1, 1, 1, 2, 140.0, 90.0, 0, 0, '');

            SET IDENTITY_INSERT GuiaRemision OFF;");
    }
}

file sealed class FixtureYearlyConnectionFactory(ManifiestoRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public SqlConnection Create(int year) => new(fixture.YearlyConnectionString);
    public SqlConnection CreateForCurrentYear() => new(fixture.YearlyConnectionString);
}

file sealed class FixtureMasterConnectionFactory(ManifiestoRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory
{
    public SqlConnection Create() => new(fixture.MasterConnectionString);
}
