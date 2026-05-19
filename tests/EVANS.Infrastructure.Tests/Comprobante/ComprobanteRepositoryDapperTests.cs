using Dapper;
using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.Comprobante;
using EVANS.Infrastructure.Sql.Comprobante;
using EVANS.Infrastructure.Sql.GuiaRemision;
using Microsoft.Data.SqlClient;
using Agg = EVANS.Domain.Comprobante.Comprobante;

namespace EVANS.Infrastructure.Tests.Comprobante;

[Collection("ComprobanteRepository")]
public class ComprobanteRepositoryDapperTests : IAsyncLifetime
{
    private readonly ComprobanteRepositoryFixture _fixture;

    public ComprobanteRepositoryDapperTests(ComprobanteRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // (a) Insertar Factura — returns Codigo > 0
    // ------------------------------------------------------------------

    [Fact]
    public void Insertar_Factura_ReturnsCodigo()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var comprobante = BuildFactura();

        int codigo;
        using (var uow = factory.Create(ComprobanteRepositoryFixture.TestYear))
        {
            codigo = repo.Insertar(comprobante, uow);
            uow.Commit();
        }

        codigo.Should().BeGreaterThan(0);
        comprobante.Codigo.Should().Be(codigo);
    }

    // ------------------------------------------------------------------
    // (b) ObtenerPorCodigo — returns fully hydrated DTO with detalles
    // ------------------------------------------------------------------

    [Fact]
    public void ObtenerPorCodigo_WithDetalles_MapsCorrectly()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var comprobante = BuildFactura(
            detalles:
            [
                new DetalleComprobante(2, "Servicio de transporte Lima-Trujillo", 100m, 200m),
                new DetalleComprobante(1, "Cargo por seguro", 50m, 50m)
            ]);

        int codigo;
        using (var uow = factory.Create(ComprobanteRepositoryFixture.TestYear))
        {
            codigo = repo.Insertar(comprobante, uow);
            uow.Commit();
        }

        var dto = repo.ObtenerPorCodigo(codigo);

        dto.Should().NotBeNull();
        dto!.Codigo.Should().Be(codigo);
        dto.Tipo.Should().Be(TipoComprobante.Factura);
        dto.ClienteCodigo.Should().Be(1);
        dto.Detalles.Should().HaveCount(2);
        dto.Detalles.Should().Contain(d => d.Descripcion == "Servicio de transporte Lima-Trujillo");
        dto.Detalles.Should().Contain(d => d.Descripcion == "Cargo por seguro");
        dto.Total.Should().Be(250m);
    }

    // ------------------------------------------------------------------
    // (c) Actualizar — replaces detalles (DELETE + re-INSERT)
    // ------------------------------------------------------------------

    [Fact]
    public async Task Actualizar_ReplacesDetalles()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var original = BuildFactura(
            detalles: [new DetalleComprobante(1, "Original", 100m, 100m)]);

        int codigo;
        using (var uow = factory.Create(ComprobanteRepositoryFixture.TestYear))
        {
            codigo = repo.Insertar(original, uow);
            uow.Commit();
        }

        // Capture original serie/numero — must NOT change after Actualizar
        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var before = await conn.QuerySingleAsync(
            "SELECT COMP_SERIE, COMP_NUMERO FROM Comprobante WHERE COMP_CODIGO = @id",
            new { id = codigo });

        var updated = BuildFactura(
            codigo: codigo,
            detalles:
            [
                new DetalleComprobante(3, "Nuevo detalle A", 50m, 150m),
                new DetalleComprobante(2, "Nuevo detalle B", 75m, 150m)
            ]);

        using (var uow = factory.Create(ComprobanteRepositoryFixture.TestYear))
        {
            repo.Actualizar(updated, uow);
            uow.Commit();
        }

        var dto = repo.ObtenerPorCodigo(codigo);
        dto!.Detalles.Should().HaveCount(2);
        dto.Detalles.Should().NotContain(d => d.Descripcion == "Original");
        dto.Detalles.Should().Contain(d => d.Descripcion == "Nuevo detalle A");
        dto.Detalles.Should().Contain(d => d.Descripcion == "Nuevo detalle B");

        // COMP_SERIE and COMP_NUMERO must be unchanged (fiscal compliance)
        var after = await conn.QuerySingleAsync(
            "SELECT COMP_SERIE, COMP_NUMERO FROM Comprobante WHERE COMP_CODIGO = @id",
            new { id = codigo });
        ((string)after.COMP_SERIE).Should().Be((string)before.COMP_SERIE);
        ((string)after.COMP_NUMERO).Should().Be((string)before.COMP_NUMERO);
    }

    // ------------------------------------------------------------------
    // (d) Eliminar — removes header and all detalles
    // ------------------------------------------------------------------

    [Fact]
    public async Task Eliminar_RemovesComprobante()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        var comprobante = BuildFactura(
            detalles:
            [
                new DetalleComprobante(1, "Det A", 10m, 10m),
                new DetalleComprobante(1, "Det B", 20m, 20m)
            ]);

        int codigo;
        using (var uow = factory.Create(ComprobanteRepositoryFixture.TestYear))
        {
            codigo = repo.Insertar(comprobante, uow);
            uow.Commit();
        }

        using (var uow = factory.Create(ComprobanteRepositoryFixture.TestYear))
        {
            repo.Eliminar(codigo, uow);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var headerCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Comprobante WHERE COMP_CODIGO = @id", new { id = codigo });
        var detalleCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DetalleComprobante WHERE COMP_CODIGO = @id", new { id = codigo });

        headerCount.Should().Be(0);
        detalleCount.Should().Be(0);
    }

    // ------------------------------------------------------------------
    // (e) ObtenerPorCodigo — not found returns null
    // ------------------------------------------------------------------

    [Fact]
    public void ObtenerPorCodigo_NotFound_ReturnsNull()
    {
        var repo = BuildRepo();

        var result = repo.ObtenerPorCodigo(999999);

        result.Should().BeNull();
    }

    // ------------------------------------------------------------------
    // (f) Buscar — by date range returns paged results
    // ------------------------------------------------------------------

    [Fact]
    public void Buscar_ByFecha_ReturnsPaged()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        // Insert 2 comprobantes
        var c1 = BuildFactura(fecha: new DateTime(2024, 6, 15));
        var c2 = BuildBoleta(fecha: new DateTime(2024, 7, 20));

        using (var uow = factory.Create(ComprobanteRepositoryFixture.TestYear))
        {
            repo.Insertar(c1, uow);
            repo.Insertar(c2, uow);
            uow.Commit();
        }

        var filtroAll = new BuscarComprobantesFiltro(
            Desde: new DateTime(2024, 6, 1),
            Hasta: new DateTime(2024, 7, 31),
            ClienteCodigo: null,
            Tipo: null,
            SoloImpreso: null);
        var all = repo.Buscar(filtroAll);
        all.Should().HaveCount(2);

        var filtroFactura = new BuscarComprobantesFiltro(
            Desde: null,
            Hasta: null,
            ClienteCodigo: null,
            Tipo: TipoComprobante.Factura,
            SoloImpreso: null);
        var facturas = repo.Buscar(filtroFactura);
        facturas.Should().OnlyContain(r => r.Tipo == TipoComprobante.Factura);

        var filtroNone = new BuscarComprobantesFiltro(
            Desde: new DateTime(2025, 1, 1),
            Hasta: new DateTime(2025, 1, 31),
            ClienteCodigo: null,
            Tipo: null,
            SoloImpreso: null);
        var none = repo.Buscar(filtroNone);
        none.Should().BeEmpty();
    }

    // ------------------------------------------------------------------
    // (g) Decimal precision — float→decimal round-trip: no precision loss
    // ------------------------------------------------------------------

    [Fact]
    public void DecimalPrecision_RoundTrip_NoLoss()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        // 118.18 is the canonical value from the spec (Total=118m, rate=0.18)
        // Use a value that is problematic for float: 118.18
        var detalles = new List<DetalleComprobante>
        {
            new(1, "Flete con decimales exactos", 0m, 118.18m)
        };

        var comprobante = BuildFactura(detalles: detalles);

        int codigo;
        using (var uow = factory.Create(ComprobanteRepositoryFixture.TestYear))
        {
            codigo = repo.Insertar(comprobante, uow);
            uow.Commit();
        }

        var dto = repo.ObtenerPorCodigo(codigo);
        dto.Should().NotBeNull();
        // SQL Server FLOAT → C# decimal via Dapper: value must round-trip with no more than 2dp loss
        dto!.Total.Should().BeApproximately(118.18m, 0.01m);
        // Detalles flete round-trip
        dto.Detalles[0].Flete.Should().BeApproximately(118.18m, 0.01m);
    }

    // ------------------------------------------------------------------
    // (h) ObtenerPorCodigo — populates RucODni from CLIENTE master DB
    // ------------------------------------------------------------------

    [Fact]
    public void ObtenerPorCodigo_WithCliente_PopulatesRucDni()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        // CLIENTE with CLIE_CODIGO=1 (RUC type, IDEN_CODIGO=1) is seeded in fixture.
        // Factura → RucODni should return the RUC stored in CLIE_NROIDENTIFICACION.
        var comprobante = BuildFactura();

        int codigo;
        using (var uow = factory.Create(ComprobanteRepositoryFixture.TestYear))
        {
            codigo = repo.Insertar(comprobante, uow);
            uow.Commit();
        }

        var dto = repo.ObtenerPorCodigo(codigo);

        dto.Should().NotBeNull();
        dto!.RucODni.Should().NotBeEmpty("RucODni should be populated from CLIENTE.CLIE_NROIDENTIFICACION");
        dto.RucODni.Should().Be("20111111111");  // Seeded value for CLIE_CODIGO=1
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private ComprobanteRepositoryDapper BuildRepo()
    {
        var yearlyFactory = new FixtureConnectionFactory(_fixture);
        var masterFactory = new FixtureMasterConnectionFactory(_fixture);
        return new ComprobanteRepositoryDapper(yearlyFactory, masterFactory);
    }

    private SqlUnitOfWorkFactory BuildUowFactory() =>
        new(new FixtureYearlyConnectionFactory(_fixture));

    private static Agg BuildFactura(
        int? codigo = null,
        DateTime? fecha = null,
        IReadOnlyList<DetalleComprobante>? detalles = null)
    {
        var d = detalles ?? [new DetalleComprobante(1, "Flete Lima-Trujillo", 100m, 100m)];
        var comprobante = Agg.CrearFactura(
            numero: new NumeroComprobante("F001", "000001"),
            fecha: fecha ?? new DateTime(2024, 6, 15),
            clienteCodigo: 1,
            ruc: "20123456789",
            direccion: "Av Lima 123|Lima|Lima",
            detalles: d,
            origen: new Standalone(),
            igvRate: 0.18m);

        if (codigo.HasValue)
            comprobante.SetCodigo(codigo.Value);

        return comprobante;
    }

    private static Agg BuildBoleta(
        int? codigo = null,
        DateTime? fecha = null,
        IReadOnlyList<DetalleComprobante>? detalles = null)
    {
        var d = detalles ?? [new DetalleComprobante(1, "Flete Boleta", 50m, 50m)];
        var comprobante = Agg.CrearBoleta(
            numero: new NumeroComprobante("B001", "000001"),
            fecha: fecha ?? new DateTime(2024, 7, 20),
            clienteCodigo: 1,
            dni: "12345678",
            direccion: "Av Arequipa 456|Lima|Lima",
            detalles: d,
            origen: new Standalone());

        if (codigo.HasValue)
            comprobante.SetCodigo(codigo.Value);

        return comprobante;
    }
}

// ------------------------------------------------------------------
// In-test adapters
// ------------------------------------------------------------------

file sealed class FixtureConnectionFactory(ComprobanteRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) =>
        new(fixture.YearlyConnectionString);

    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() =>
        new(fixture.YearlyConnectionString);
}

file sealed class FixtureYearlyConnectionFactory(ComprobanteRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) =>
        new(fixture.YearlyConnectionString);

    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() =>
        new(fixture.YearlyConnectionString);
}

file sealed class FixtureMasterConnectionFactory(ComprobanteRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create() =>
        new(fixture.MasterConnectionString);
}
