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
    public async Task ListarDestinosActivos_ReturnsOnlyActiveDestinationsOrderedByName()
    {
        await SeedDestinosAsync();

        var repository = new ReportesConsultaRepositorySql(
            new FixtureYearlyConnectionFactory(_fixture),
            new FixtureMasterConnectionFactory(_fixture));

        var result = await repository.ListarDestinosActivosAsync(CancellationToken.None);

        result.Select(d => d.Nombre).Should().ContainInOrder("Arequipa", "Lima");
        result.Should().NotContain(d => d.Nombre == "Inactive");
    }

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

    [Fact]
    public async Task ConsultarGuiasPorCliente_WhenPendingOnly_FiltersByClientDatesAndSentFlag()
    {
        await SeedClientesAsync();
        await SeedGuiasAsync();

        var repository = new ReportesConsultaRepositorySql(
            new FixtureYearlyConnectionFactory(_fixture),
            new FixtureMasterConnectionFactory(_fixture));

        var filtro = new GuiasPorClienteFiltro(
            Year: ManifiestoRepositoryFixture.TestYear,
            ClienteCodigo: 2,
            FechaDesde: new DateTime(2024, 6, 1),
            FechaHasta: new DateTime(2024, 6, 30),
            SoloPendientes: true);

        var result = await repository.ConsultarGuiasPorClienteAsync(
            filtro,
            ManifiestoRepositoryFixture.TestYear,
            CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(guia => !guia.Enviado);
        result[0].NroDoc.Should().Be("GR01-000013");
        result[0].Remitente.Should().Be("Cliente Uno");
        result[0].Destinatario.Should().Be("Cliente Dos");
    }

    [Fact]
    public async Task ConsultarGuiasPorCliente_WhenIncludeSent_ReturnsSentGuidesToo()
    {
        await SeedClientesAsync();
        await SeedGuiasAsync();

        var repository = new ReportesConsultaRepositorySql(
            new FixtureYearlyConnectionFactory(_fixture),
            new FixtureMasterConnectionFactory(_fixture));

        var filtro = new GuiasPorClienteFiltro(
            Year: ManifiestoRepositoryFixture.TestYear,
            ClienteCodigo: 2,
            FechaDesde: new DateTime(2024, 6, 1),
            FechaHasta: new DateTime(2024, 6, 30),
            SoloPendientes: false);

        var result = await repository.ConsultarGuiasPorClienteAsync(
            filtro,
            ManifiestoRepositoryFixture.TestYear,
            CancellationToken.None);

        result.Should().Contain(guia => guia.NroDoc == "GR01-000015" && guia.Enviado);
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

    private async Task SeedDestinosAsync()
    {
        using var conn = new SqlConnection(_fixture.MasterConnectionString);
        await conn.OpenAsync();

        await conn.ExecuteAsync(@"
            SET IDENTITY_INSERT ESTADO ON;
            IF NOT EXISTS (SELECT 1 FROM ESTADO WHERE ESTA_CODIGO = 2)
                INSERT INTO ESTADO (ESTA_CODIGO, ESTA_DESCRIPCION) VALUES (2, 'Inactivo');
            SET IDENTITY_INSERT ESTADO OFF;

            IF EXISTS (SELECT 1 FROM DESTINO WHERE DEST_CODIGO IN (20, 21, 22))
                DELETE FROM DESTINO WHERE DEST_CODIGO IN (20, 21, 22);

            SET IDENTITY_INSERT DESTINO ON;
            INSERT INTO DESTINO (DEST_CODIGO, DEST_NOMBRE, DEST_DISTANCIAVIRTUAL, ESTA_CODIGO)
            VALUES
                (20, 'Lima', 0, 1),
                (21, 'Arequipa', 0, 1),
                (22, 'Inactive', 0, 2);
            SET IDENTITY_INSERT DESTINO OFF;");
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
                ESTA_CODIGO, GREM_BULTOS, GREM_COSTOTOTAL, GREM_PESOTOTAL,
                GREM_ENVIADO, GREM_MANIFIESTO, GREM_NROMANIFIESTO
            )
            VALUES
                (10, 'GR01', '000010', '2024-06-10', '2024-06-11', 1, 1, 1, 1, 1, 1, 1, 1, 2, 100.0, 50.0, 0, 0, ''),
                (11, 'GR01', '000011', '2024-06-20', '2024-06-21', 1, 1, 1, 1, 1, 1, 1, 1, 3, 110.0, 60.0, 0, 0, ''),
                (12, 'GR01', '000012', '2024-07-01', '2024-07-02', 1, 1, 1, 1, 1, 1, 1, 1, 4, 120.0, 70.0, 0, 0, ''),
                (13, 'GR01', '000013', '2024-06-15', '2024-06-16', 1, 2, 2, 1, 1, 1, 1, 1, 5, 130.0, 80.0, 0, 0, ''),
                (14, 'GR01', '000014', '2024-06-18', '2024-06-19', 1, 2, 1, 1, 1, 1, 1, 2, 6, 140.0, 90.0, 0, 0, ''),
                (15, 'GR01', '000015', '2024-06-22', '2024-06-23', 1, 2, 2, 1, 1, 1, 1, 1, 7, 150.0, 95.0, 1, 0, '');

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
