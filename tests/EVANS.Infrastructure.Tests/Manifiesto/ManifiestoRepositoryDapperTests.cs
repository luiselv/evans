using Dapper;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Infrastructure.Sql.GuiaRemision;
using EVANS.Infrastructure.Sql.Manifiesto;
using Microsoft.Data.SqlClient;
using Agg = EVANS.Domain.Manifiesto.Manifiesto;

namespace EVANS.Infrastructure.Tests.Manifiesto;

[Collection("ManifiestoRepository")]
public class ManifiestoRepositoryDapperTests : IAsyncLifetime
{
    private readonly ManifiestoRepositoryFixture _fixture;

    public ManifiestoRepositoryDapperTests(ManifiestoRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // I-01: insertar_manifiesto_asigna_codigo
    // INSERT round-trip, Codigo set via SCOPE_IDENTITY
    // ------------------------------------------------------------------

    [Fact]
    public async Task Insertar_Manifiesto_AsignaCodigo()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var manifiesto = BuildManifiesto(guiaIds: [1]);

        int codigo;
        using (var uow = factory.Create(ManifiestoRepositoryFixture.TestYear))
        {
            codigo = await repo.InsertarAsync(manifiesto, uow, CancellationToken.None);
            uow.Commit();
        }

        codigo.Should().BeGreaterThan(0);
        manifiesto.Codigo.Should().Be(codigo);
    }

    // ------------------------------------------------------------------
    // I-02: insertar_manifiesto_con_detalle_persiste_guias
    // DetalleManifiesto rows created correctly
    // ------------------------------------------------------------------

    [Fact]
    public async Task Insertar_ManifiestoConDetalle_PersistGuias()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var manifiesto = BuildManifiesto(guiaIds: [1, 2]);

        int codigo;
        using (var uow = factory.Create(ManifiestoRepositoryFixture.TestYear))
        {
            codigo = await repo.InsertarAsync(manifiesto, uow, CancellationToken.None);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var detalleCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DetalleManifiesto WHERE MANI_CODIGO = @codigo",
            new { codigo });

        detalleCount.Should().Be(2);
    }

    // ------------------------------------------------------------------
    // I-02b: ObtenerPorCodigo returns hydrated DTO with lineas
    // ------------------------------------------------------------------

    [Fact]
    public async Task ObtenerPorCodigo_RetornaDto_ConLineas()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var manifiesto = BuildManifiesto(guiaIds: [1, 2]);

        int codigo;
        using (var uow = factory.Create(ManifiestoRepositoryFixture.TestYear))
        {
            codigo = await repo.InsertarAsync(manifiesto, uow, CancellationToken.None);
            uow.Commit();
        }

        var dto = await repo.ObtenerPorCodigoAsync(codigo, ManifiestoRepositoryFixture.TestYear, CancellationToken.None);

        dto.Should().NotBeNull();
        dto!.Codigo.Should().Be(codigo);
        dto.TransportistaCodigo.Should().Be(1);
        dto.VehiculoCodigo.Should().Be(1);
        dto.ChoferCodigo.Should().Be(1);
        dto.Lineas.Should().HaveCount(2);
    }

    // ------------------------------------------------------------------
    // I-03a: actualizar_manifiesto_reemplaza_detalle
    // Old DetalleManifiesto rows deleted, new ones inserted
    // ------------------------------------------------------------------

    [Fact]
    public async Task Actualizar_ManifiestoReemplazaDetalle()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        // Insert with guia 1 only
        var original = BuildManifiesto(guiaIds: [1]);
        int codigo;
        using (var uow = factory.Create(ManifiestoRepositoryFixture.TestYear))
        {
            codigo = await repo.InsertarAsync(original, uow, CancellationToken.None);
            uow.Commit();
        }

        // Update with guia 2 only
        var updated = BuildManifiesto(guiaIds: [2]);
        updated.SetCodigo(codigo);

        using (var uow = factory.Create(ManifiestoRepositoryFixture.TestYear))
        {
            await repo.ActualizarAsync(updated, uow, CancellationToken.None);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var guiaIds = (await conn.QueryAsync<int>(
            "SELECT GREM_CODIGO FROM DetalleManifiesto WHERE MANI_CODIGO = @codigo",
            new { codigo })).ToList();

        guiaIds.Should().BeEquivalentTo(new[] { 2 });
    }

    // ------------------------------------------------------------------
    // I-03b: eliminar_manifiesto_cascades_detalle
    // Both Manifiesto and DetalleManifiesto rows deleted
    // ------------------------------------------------------------------

    [Fact]
    public async Task Eliminar_ManifiestoEliminarDetalle()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var manifiesto = BuildManifiesto(guiaIds: [1, 2]);
        int codigo;
        using (var uow = factory.Create(ManifiestoRepositoryFixture.TestYear))
        {
            codigo = await repo.InsertarAsync(manifiesto, uow, CancellationToken.None);
            uow.Commit();
        }

        using (var uow = factory.Create(ManifiestoRepositoryFixture.TestYear))
        {
            await repo.EliminarAsync(codigo, uow, CancellationToken.None);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var headerCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Manifiesto WHERE MANI_CODIGO = @id", new { id = codigo });
        var detalleCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DetalleManifiesto WHERE MANI_CODIGO = @id", new { id = codigo });

        headerCount.Should().Be(0);
        detalleCount.Should().Be(0);
    }

    // ------------------------------------------------------------------
    // BuscarAsync — returns list filtered by date
    // ------------------------------------------------------------------

    [Fact]
    public async Task Buscar_PorFecha_RetornaFiltrado()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        // Insert two manifiestos on different dates
        var m1 = BuildManifiesto(fecha: new DateTime(2024, 6, 15), guiaIds: [1]);
        var m2 = BuildManifiesto(fecha: new DateTime(2024, 7, 20), guiaIds: [2]);

        using (var uow = factory.Create(ManifiestoRepositoryFixture.TestYear))
        {
            await repo.InsertarAsync(m1, uow, CancellationToken.None);
            await repo.InsertarAsync(m2, uow, CancellationToken.None);
            uow.Commit();
        }

        var filtroAll = new EVANS.Application.Manifiesto.DTOs.BuscarManifiestosFiltro(
            Year: ManifiestoRepositoryFixture.TestYear,
            FechaDesde: new DateTime(2024, 6, 1),
            FechaHasta: new DateTime(2024, 7, 31));

        var all = await repo.BuscarAsync(filtroAll, ManifiestoRepositoryFixture.TestYear, CancellationToken.None);
        all.Should().HaveCount(2);

        var filtroJunio = new EVANS.Application.Manifiesto.DTOs.BuscarManifiestosFiltro(
            Year: ManifiestoRepositoryFixture.TestYear,
            FechaDesde: new DateTime(2024, 6, 1),
            FechaHasta: new DateTime(2024, 6, 30));

        var junio = await repo.BuscarAsync(filtroJunio, ManifiestoRepositoryFixture.TestYear, CancellationToken.None);
        junio.Should().HaveCount(1);
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private ManifiestoRepositoryDapper BuildRepo()
    {
        var yearlyFactory = new FixtureYearlyConnectionFactory(_fixture);
        var masterFactory = new FixtureMasterConnectionFactory(_fixture);
        return new ManifiestoRepositoryDapper(yearlyFactory, masterFactory);
    }

    private SqlUnitOfWorkFactory BuildUowFactory() =>
        new(new FixtureYearlyConnectionFactory(_fixture));

    private static Agg BuildManifiesto(
        IReadOnlyList<int>? guiaIds = null,
        DateTime? fecha = null)
    {
        return Agg.Crear(
            fecha: fecha ?? new DateTime(2024, 6, 15),
            transportistaCodigo: 1,
            vehiculoCodigo: 1,
            carretaCodigo: 1,
            choferCodigo: 1,
            importe: 500m,
            peso: 100m,
            estadoCodigo: 1,
            usuarioCodigo: 1,
            guiaIds: guiaIds ?? [1]);
    }
}

// ------------------------------------------------------------------
// In-test connection adapters
// ------------------------------------------------------------------

file sealed class FixtureYearlyConnectionFactory(ManifiestoRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) =>
        new(fixture.YearlyConnectionString);

    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() =>
        new(fixture.YearlyConnectionString);
}

file sealed class FixtureMasterConnectionFactory(ManifiestoRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create() =>
        new(fixture.MasterConnectionString);
}
