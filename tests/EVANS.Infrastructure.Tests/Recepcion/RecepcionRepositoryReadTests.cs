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
public class RecepcionRepositoryReadTests : IAsyncLifetime
{
    private readonly RecepcionRepositoryFixture _fixture;

    public RecepcionRepositoryReadTests(RecepcionRepositoryFixture fixture) => _fixture = fixture;
    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    private RecepcionRepositoryDapper BuildRepo()
    {
        var yearlyFactory = new FixedConnectionFactory(_fixture.YearlyConnectionString);
        var masterFactory = new FixedMasterConnectionFactory(_fixture.MasterConnectionString);
        return new RecepcionRepositoryDapper(yearlyFactory, masterFactory);
    }

    private SqlUnitOfWorkFactory BuildUowFactory() =>
        new SqlUnitOfWorkFactory(new FixedConnectionFactory(_fixture.YearlyConnectionString));

    private async Task<int> InsertRecepcionOnDate(DateTime fecha)
    {
        var det = Det.Crear(1m, "Item", 0m, "UND", 50m, "GR", "001");
        var agg = Agg.Crear(fecha, 1, TipoDireccion.Agencia, "Dir", 2,
            TipoDireccion.Agencia, "Dir2", 1, 1, null, null, 50m, null, 1, new[] { det });

        var repo = BuildRepo();
        var factory = BuildUowFactory();

        using var uow = factory.Create(RecepcionRepositoryFixture.TestYear);
        var codigo = await repo.CrearAsync(agg, uow, CancellationToken.None);
        uow.Commit();
        return codigo;
    }

    // ------------------------------------------------------------------
    // I-06: BuscarPorRangoFechasAsync — date filter
    // ------------------------------------------------------------------

    [Fact]
    public async Task Buscar_PorRangoFechas_RetornasoloLosDelRango()
    {
        var mayFirst  = new DateTime(2024, 5,  1, 12, 0, 0);
        var may15     = new DateTime(2024, 5, 15, 12, 0, 0);
        var juneFirst = new DateTime(2024, 6,  1, 12, 0, 0);

        await InsertRecepcionOnDate(mayFirst);
        await InsertRecepcionOnDate(may15);
        await InsertRecepcionOnDate(juneFirst);

        var rango = DateRange.Intervalo(new DateTime(2024, 5, 1), new DateTime(2024, 5, 31));

        var repo = BuildRepo();
        var results = await repo.BuscarPorRangoFechasAsync(rango, RecepcionRepositoryFixture.TestYear, CancellationToken.None);

        results.Should().HaveCount(2);
        results.Should().AllSatisfy(r => r.Fecha.Month.Should().Be(5));
    }

    // ------------------------------------------------------------------
    // I-08: RECE_GUIAREMISION is never written by the repository
    // ------------------------------------------------------------------

    [Fact]
    public async Task Crear_NoEscribeGuiaRemisionVinculada()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();
        var det = Det.Crear(1m, "Item", 0m, "UND", 50m, "GR", "001");
        var agg = Agg.Crear(DateTime.Today, 1, TipoDireccion.Agencia, "Dir", 2,
            TipoDireccion.Agencia, "Dir2", 1, 1, null, null, 50m, null, 1, new[] { det });

        int codigo;
        using (var uow = factory.Create(RecepcionRepositoryFixture.TestYear))
        {
            codigo = await repo.CrearAsync(agg, uow, CancellationToken.None);
            uow.Commit();
        }

        using var conn = new SqlConnection(_fixture.YearlyConnectionString);
        var guiaRef = await conn.ExecuteScalarAsync<string?>(
            "SELECT RECE_GUIAREMISION FROM Recepcion WHERE RECE_CODIGO = @codigo", new { codigo });

        guiaRef.Should().BeNullOrEmpty();
    }
}
