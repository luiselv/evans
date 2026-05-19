using Dapper;
using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.GuiaRemision;
using EVANS.Infrastructure.Sql.GuiaRemision;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Tests.GuiaRemision;

[Collection("GuiaRepository")]
public class GuiaRepositoryDapperTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public GuiaRepositoryDapperTests(GuiaRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // (a) Insertar + ObtenerPorCodigo — fully hydrated
    // ------------------------------------------------------------------

    [Fact]
    public Task Insertar_then_ObtenerPorCodigo_returns_fully_hydrated_guia()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var guia = BuildGuia(hasManifest: false,
            detalles: [BuildDetalle("Caja de mangos", 10), BuildDetalle("Caja de uvas", 5)]);

        int codigo;
        using (var uow = factory.Create(GuiaRepositoryFixture.TestYear))
        {
            codigo = repo.Insertar(guia, uow);
            uow.Commit();
        }

        var dto = repo.ObtenerPorCodigo(codigo, GuiaRepositoryFixture.TestYear);

        dto.Should().NotBeNull();
        dto!.Codigo.Should().Be(codigo);
        dto.Detalles.Should().HaveCount(2);
        dto.Detalles.Should().Contain(d => d.Descripcion == "Caja de mangos");
        dto.Detalles.Should().Contain(d => d.Descripcion == "Caja de uvas");
        dto.RemitenteId.Should().Be(1);
        dto.DestinatarioId.Should().Be(2);
        dto.NumeroGuia.Should().Be("GR01-000001");
        return Task.CompletedTask;
    }

    // ------------------------------------------------------------------
    // (b) GREM_MANIFIESTO=1 → HasManifest=true
    // ------------------------------------------------------------------

    [Fact]
    public async Task GREM_MANIFIESTO_1_maps_to_HasManifest_true()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var guia = BuildGuia(hasManifest: true, vehiculoId: 1, carretaId: 1, choferId: 1,
            detalles: [BuildDetalle("Pallet A", 1)]);

        int codigoB;
        using (var uow = factory.Create(GuiaRepositoryFixture.TestYear))
        {
            codigoB = repo.Insertar(guia, uow);
            uow.Commit();
        }

        // Verify DB column value directly
        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var dbValue = await conn.ExecuteScalarAsync<int>(
            "SELECT GREM_MANIFIESTO FROM GuiaRemision WHERE GREM_CODIGO = @id",
            new { id = codigoB });
        dbValue.Should().Be(1);

        // Verify DTO mapping
        var dto = repo.ObtenerPorCodigo(codigoB, GuiaRepositoryFixture.TestYear);
        dto!.HasManifest.Should().BeTrue();
    }

    // ------------------------------------------------------------------
    // (c) GREM_MANIFIESTO=0 → HasManifest=false
    // ------------------------------------------------------------------

    [Fact]
    public async Task GREM_MANIFIESTO_0_maps_to_HasManifest_false()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var guia = BuildGuia(hasManifest: false, detalles: [BuildDetalle("Cajita sin manifiesto", 1)]);

        int codigoC;
        using (var uow = factory.Create(GuiaRepositoryFixture.TestYear))
        {
            codigoC = repo.Insertar(guia, uow);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var dbValue = await conn.ExecuteScalarAsync<int>(
            "SELECT GREM_MANIFIESTO FROM GuiaRemision WHERE GREM_CODIGO = @id",
            new { id = codigoC });
        dbValue.Should().Be(0);

        var dto = repo.ObtenerPorCodigo(codigoC, GuiaRepositoryFixture.TestYear);
        dto!.HasManifest.Should().BeFalse();
    }

    // ------------------------------------------------------------------
    // (d) Actualizar replaces detalles — grem_serie/numero untouched
    // ------------------------------------------------------------------

    [Fact]
    public async Task Actualizar_replaces_detalles()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        // Insert with 1 detalle
        var guia = BuildGuia(hasManifest: false, detalles: [BuildDetalle("Original", 1)]);
        int codigo;
        using (var uow = factory.Create(GuiaRepositoryFixture.TestYear))
        {
            codigo = repo.Insertar(guia, uow);
            uow.Commit();
        }

        // Capture original serie/numero before update
        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var before = await conn.QuerySingleAsync(
            "SELECT GREM_SERIE, GREM_NUMERO FROM GuiaRemision WHERE GREM_CODIGO = @id",
            new { id = codigo });

        // Build updated guia (same code, new numero would be ignored on UPDATE per design)
        var updatedGuia = BuildGuia(
            codigo: codigo,
            hasManifest: false,
            detalles: [BuildDetalle("Updated A", 2), BuildDetalle("Updated B", 3)]);

        using (var uow = factory.Create(GuiaRepositoryFixture.TestYear))
        {
            repo.Actualizar(updatedGuia, uow);
            uow.Commit();
        }

        // Assert: 2 detalles, old one gone
        var dto = repo.ObtenerPorCodigo(codigo, GuiaRepositoryFixture.TestYear);
        dto!.Detalles.Should().HaveCount(2);
        dto.Detalles.Should().NotContain(d => d.Descripcion == "Original");
        dto.Detalles.Should().Contain(d => d.Descripcion == "Updated A");
        dto.Detalles.Should().Contain(d => d.Descripcion == "Updated B");

        // Assert: grem_serie and grem_numero NOT changed (fiscal compliance)
        var after = await conn.QuerySingleAsync(
            "SELECT GREM_SERIE, GREM_NUMERO FROM GuiaRemision WHERE GREM_CODIGO = @id",
            new { id = codigo });
        ((string)after.GREM_SERIE).Should().Be((string)before.GREM_SERIE);
        ((string)after.GREM_NUMERO).Should().Be((string)before.GREM_NUMERO);
    }

    // ------------------------------------------------------------------
    // (e) Eliminar cascade + idempotent
    // ------------------------------------------------------------------

    [Fact]
    public async Task Eliminar_cascade_then_idempotent()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        // Insert with 3 detalles
        var guia = BuildGuia(hasManifest: false,
            detalles: [BuildDetalle("A", 1), BuildDetalle("B", 2), BuildDetalle("C", 3)]);

        int codigo;
        using (var uow = factory.Create(GuiaRepositoryFixture.TestYear))
        {
            codigo = repo.Insertar(guia, uow);
            uow.Commit();
        }

        // First delete
        using (var uow = factory.Create(GuiaRepositoryFixture.TestYear))
        {
            repo.Eliminar(codigo, uow);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var guiaCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM GuiaRemision WHERE GREM_CODIGO = @id", new { id = codigo });
        var detalleCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DetalleGuia WHERE GREM_CODIGO = @id", new { id = codigo });

        guiaCount.Should().Be(0);
        detalleCount.Should().Be(0);

        // Second delete — idempotent, no exception
        var ex = Record.Exception(() =>
        {
            using var uow = factory.Create(GuiaRepositoryFixture.TestYear);
            repo.Eliminar(codigo, uow);
            uow.Commit();
        });
        ex.Should().BeNull();
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private GuiaRepositoryDapper BuildRepo()
    {
        var factory = new FixtureConnectionFactory(_fixture);
        return new GuiaRepositoryDapper(factory, factory);
    }

    private SqlUnitOfWorkFactory BuildUowFactory() =>
        new(new FixtureYearlyConnectionFactory(_fixture));

    private static Guia BuildGuia(
        bool hasManifest,
        IEnumerable<DetalleGuia> detalles,
        int? codigo = null,
        int? vehiculoId = null,
        int? carretaId = null,
        int? choferId = null) =>
        new(
            codigo: codigo,
            numero: new NumeroGuia("GR01", 1),
            fecha: DateTime.Today,
            remitenteId: 1,
            destinatarioId: 2,
            direccionPartida: Direccion.Parse("Av Lima 123|Lima|Lima"),
            direccionLlegada: Direccion.Parse("Av Arequipa 456|Arequipa|Arequipa"),
            hasManifest: hasManifest,
            vehiculoId: vehiculoId,
            carretaId: carretaId,
            choferId: choferId,
            igv: 0.18m,
            detalles: detalles);

    private static DetalleGuia BuildDetalle(string descripcion, int cantidad) =>
        new(
            codigo: null,
            descripcion: descripcion,
            peso: new Peso(10m),
            precioUnitario: 5.00m,
            precioTotal: 5.00m * cantidad,
            cantidad: cantidad);
}

// ------------------------------------------------------------------
// In-test adapters that wire fixture connection strings to the interfaces
// ------------------------------------------------------------------

file sealed class FixtureConnectionFactory(GuiaRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory,
      EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create() =>
        new(fixture.MasterConnectionString);

    public Microsoft.Data.SqlClient.SqlConnection Create(int year) =>
        new(fixture.YearlyConnectionString);

    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() =>
        new(fixture.YearlyConnectionString);
}

file sealed class FixtureYearlyConnectionFactory(GuiaRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) =>
        new(fixture.YearlyConnectionString);

    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() =>
        new(fixture.YearlyConnectionString);
}
