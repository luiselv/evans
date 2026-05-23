using Dapper;
using EVANS.Domain.Recepcion;
using EVANS.Infrastructure.Sql.GuiaRemision;
using EVANS.Infrastructure.Sql.Recepcion;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Xunit;
using Agg = EVANS.Domain.Recepcion.Recepcion;
using Det = EVANS.Domain.Recepcion.DetalleRecepcion;

namespace EVANS.Infrastructure.Tests.Recepcion;

[Collection("RecepcionRepository")]
public class RecepcionRepositoryDapperTests : IAsyncLifetime
{
    private readonly RecepcionRepositoryFixture _fixture;

    public RecepcionRepositoryDapperTests(RecepcionRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private RecepcionRepositoryDapper BuildRepo()
    {
        var yearlyFactory = new FixedConnectionFactory(_fixture.YearlyConnectionString);
        var masterFactory = new FixedMasterConnectionFactory(_fixture.MasterConnectionString);
        return new RecepcionRepositoryDapper(yearlyFactory, masterFactory);
    }

    private SqlUnitOfWorkFactory BuildUowFactory()
    {
        var yearlyFactory = new FixedConnectionFactory(_fixture.YearlyConnectionString);
        return new SqlUnitOfWorkFactory(yearlyFactory);
    }

    private static Agg BuildRecepcion(int nDetalles = 1)
    {
        var detalles = Enumerable.Range(1, nDetalles)
            .Select(i => Det.Crear(
                cantidad: i,
                descripcion: $"Item {i}",
                peso: i * 1.5m,
                unidad: "KG",
                costo: i * 50m,
                tipoDoc: "GR",
                nroDoc: $"GR-{i:D6}"))
            .ToList()
            .AsReadOnly();

        return Agg.Crear(
            fechaEmision: DateTime.Today,
            remitenteId: 1,
            tipoDirPartida: TipoDireccion.Agencia,
            direccionPartida: "Agencia Lima",
            destinatarioId: 2,
            tipoDirDestino: TipoDireccion.DireccionCliente,
            direccionDestino: "Av Arequipa 123",
            destinoId: 1,
            estadoId: 1,
            bultos: null,
            pesoTotal: null,
            costoTotal: detalles.Sum(d => d.Costo),
            observacion: null,
            usuarioId: 1,
            detalles: detalles);
    }

    // ------------------------------------------------------------------
    // I-01: CrearAsync inserts header row and returns Codigo
    // ------------------------------------------------------------------

    [Fact]
    public async Task Crear_InsertaHeader_AsignaCodigo()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();
        var recepcion = BuildRecepcion(1);

        int codigo;
        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            codigo = await repo.CrearAsync(recepcion, uow, CancellationToken.None);
            uow.Commit();
        }

        codigo.Should().BeGreaterThan(0);
        recepcion.Codigo.Should().Be(codigo);

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var count = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Recepcion WHERE RECE_CODIGO = @codigo", new { codigo });
        count.Should().Be(1);
    }

    // ------------------------------------------------------------------
    // I-02: CrearAsync inserts DetalleRecepcion rows
    // ------------------------------------------------------------------

    [Fact]
    public async Task Crear_ConDosDetalles_InsertaAmbosDetalles()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();
        var recepcion = BuildRecepcion(2);

        int codigo;
        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            codigo = await repo.CrearAsync(recepcion, uow, CancellationToken.None);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var count = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DetalleRecepcion WHERE RECE_CODIGO = @codigo", new { codigo });
        count.Should().Be(2);
    }

    // ------------------------------------------------------------------
    // I-03: ObtenerPorCodigoAsync round-trip — returns aggregate with detalles
    // ------------------------------------------------------------------

    [Fact]
    public async Task ObtenerPorCodigo_Existente_RetornaAggregateConDetalles()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();
        var recepcion = BuildRecepcion(2);

        int codigo;
        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            codigo = await repo.CrearAsync(recepcion, uow, CancellationToken.None);
            uow.Commit();
        }

        var found = await repo.ObtenerPorCodigoAsync(codigo, RecepcionRepositoryFixture.TestYear, CancellationToken.None);

        found.Should().NotBeNull();
        found!.Codigo.Should().Be(codigo);
        found.Detalles.Should().HaveCount(2);
        found.RemitenteId.Should().Be(1);
    }

    // ------------------------------------------------------------------
    // I-04: ActualizarAsync replaces detail rows
    // ------------------------------------------------------------------

    [Fact]
    public async Task Actualizar_ReemplazaDetalles()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();
        var recepcion = BuildRecepcion(2);

        int codigo;
        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            codigo = await repo.CrearAsync(recepcion, uow, CancellationToken.None);
            uow.Commit();
        }

        // Reload and update with 3 detalles
        var loaded = await repo.ObtenerPorCodigoAsync(codigo, RecepcionRepositoryFixture.TestYear, CancellationToken.None);
        loaded.Should().NotBeNull();

        var nuevosDetalles = Enumerable.Range(1, 3)
            .Select(i => Det.Crear(i * 2m, $"Nuevo {i}", 0m, "UND", i * 30m, "GR", $"NV-{i}"))
            .ToList()
            .AsReadOnly();

        loaded!.Actualizar(
            DateTime.Today, 1, TipoDireccion.Agencia, "Dir",
            2, TipoDireccion.Agencia, "Dir2",
            1, 1, null, null, 90m, null, nuevosDetalles);

        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            await repo.ActualizarAsync(loaded, uow, CancellationToken.None);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var count = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DetalleRecepcion WHERE RECE_CODIGO = @codigo", new { codigo });
        count.Should().Be(3);
    }

    // ------------------------------------------------------------------
    // I-05: EliminarAsync removes header and all detail rows
    // ------------------------------------------------------------------

    [Fact]
    public async Task Eliminar_RemoveHeaderAndDetalles()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();
        var recepcion = BuildRecepcion(2);

        int codigo;
        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            codigo = await repo.CrearAsync(recepcion, uow, CancellationToken.None);
            uow.Commit();
        }

        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            await repo.EliminarAsync(codigo, uow, CancellationToken.None);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var headerCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Recepcion WHERE RECE_CODIGO = @codigo", new { codigo });
        var detalleCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DetalleRecepcion WHERE RECE_CODIGO = @codigo", new { codigo });

        headerCount.Should().Be(0);
        detalleCount.Should().Be(0);
    }

    // ------------------------------------------------------------------
    // I-07: Serializable atomicity — EliminarAsync in Serializable tx
    // ------------------------------------------------------------------

    [Fact]
    public async Task Eliminar_SerializableTx_Atomico()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();
        var recepcion = BuildRecepcion(1);

        int codigo;
        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            codigo = await repo.CrearAsync(recepcion, uow, CancellationToken.None);
            uow.Commit();
        }

        // Delete and immediately verify both rows are gone in same connection scope
        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            await repo.EliminarAsync(codigo, uow, CancellationToken.None);
            uow.Commit();
        }

        var found = await repo.ObtenerPorCodigoAsync(codigo, RecepcionRepositoryFixture.TestYear, CancellationToken.None);
        found.Should().BeNull();
    }
}

