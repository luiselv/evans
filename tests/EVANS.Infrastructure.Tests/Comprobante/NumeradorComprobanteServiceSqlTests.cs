using EVANS.Domain.Comprobante;
using EVANS.Infrastructure.Sql.Comprobante;

namespace EVANS.Infrastructure.Tests.Comprobante;

[Collection("ComprobanteRepository")]
public class NumeradorComprobanteServiceSqlTests : IAsyncLifetime
{
    private readonly ComprobanteRepositoryFixture _fixture;

    public NumeradorComprobanteServiceSqlTests(ComprobanteRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // (a) Factura increment — PARA_FACTNRO1 increases each call
    // ------------------------------------------------------------------

    [Fact]
    public async Task IncrementarYObtenerAsync_Factura_IncrementsFACTNRO1()
    {
        var service = BuildService();

        var first  = await service.IncrementarYObtenerAsync(TipoComprobante.Factura);
        var second = await service.IncrementarYObtenerAsync(TipoComprobante.Factura);

        // Second call must return a strictly higher numeric value
        int.Parse(second.Numero).Should().BeGreaterThan(int.Parse(first.Numero));
    }

    // ------------------------------------------------------------------
    // (b) Boleta increment — PARA_BOLNRO1 increments independently of Factura
    // ------------------------------------------------------------------

    [Fact]
    public async Task IncrementarYObtenerAsync_Boleta_IncrementsBOLNRO1()
    {
        var service = BuildService();

        var first  = await service.IncrementarYObtenerAsync(TipoComprobante.Boleta);
        var second = await service.IncrementarYObtenerAsync(TipoComprobante.Boleta);

        int.Parse(second.Numero).Should().BeGreaterThan(int.Parse(first.Numero));
    }

    // ------------------------------------------------------------------
    // (c) Factura and Boleta counters are independent
    // ------------------------------------------------------------------

    [Fact]
    public async Task IncrementarYObtenerAsync_FacturaAndBoletaCounters_AreIndependent()
    {
        var service = BuildService();

        var factura = await service.IncrementarYObtenerAsync(TipoComprobante.Factura);
        var boleta  = await service.IncrementarYObtenerAsync(TipoComprobante.Boleta);

        // Series must differ (F001 vs B001)
        factura.Serie.Should().NotBe(boleta.Serie);
    }

    // ------------------------------------------------------------------
    // (d) Returns correct NumeroComprobante with Serie from PARA_FACTSERIE
    // ------------------------------------------------------------------

    [Fact]
    public async Task IncrementarYObtenerAsync_Factura_ReturnsCorrectSerieFromParametros()
    {
        var service = BuildService();

        var result = await service.IncrementarYObtenerAsync(TipoComprobante.Factura);

        result.Serie.Should().Be("F001");  // Seeded value
        result.Numero.Should().MatchRegex(@"^\d{6}$");  // 6-digit zero-padded
    }

    // ------------------------------------------------------------------
    // (e) Returns correct NumeroComprobante with Serie from PARA_BOLSERIE
    // ------------------------------------------------------------------

    [Fact]
    public async Task IncrementarYObtenerAsync_Boleta_ReturnsCorrectSerieFromParametros()
    {
        var service = BuildService();

        var result = await service.IncrementarYObtenerAsync(TipoComprobante.Boleta);

        result.Serie.Should().Be("B001");  // Seeded value
        result.Numero.Should().MatchRegex(@"^\d{6}$");
    }

    // ------------------------------------------------------------------
    // (f) Concurrent calls produce no duplicates (5 parallel increments)
    // ------------------------------------------------------------------

    [Fact]
    public async Task IncrementarYObtenerAsync_ConcurrentCalls_NoDuplicates()
    {
        var results = new System.Collections.Concurrent.ConcurrentBag<string>();

        await Parallel.ForEachAsync(Enumerable.Range(0, 5), async (_, ct) =>
        {
            var service = BuildService();
            var result = await service.IncrementarYObtenerAsync(TipoComprobante.Factura, ct);
            results.Add(result.Numero);
        });

        results.Should().OnlyHaveUniqueItems();
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private NumeradorComprobanteServiceSql BuildService() =>
        new(new FixtureMasterConnectionFactoryForNumerador(_fixture));
}

file sealed class FixtureMasterConnectionFactoryForNumerador(ComprobanteRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create() =>
        new(fixture.MasterConnectionString);
}
