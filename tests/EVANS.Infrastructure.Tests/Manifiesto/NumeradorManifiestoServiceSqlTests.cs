using EVANS.Domain.Manifiesto;
using EVANS.Infrastructure.Sql.Manifiesto;

namespace EVANS.Infrastructure.Tests.Manifiesto;

[Collection("ManifiestoRepository")]
public class NumeradorManifiestoServiceSqlTests : IAsyncLifetime
{
    private readonly ManifiestoRepositoryFixture _fixture;

    public NumeradorManifiestoServiceSqlTests(ManifiestoRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // I-04: incrementar_retorna_numero_con_formato_anio
    // Returns consecutive NumeroManifiesto with format {YYYY}-{N}
    // ------------------------------------------------------------------

    [Fact]
    public async Task IncrementarYObtenerAsync_RetornaNumeroConFormatoAnio()
    {
        var service = BuildService();
        const int year = 2024;

        var first = await service.IncrementarYObtenerAsync(year);
        var second = await service.IncrementarYObtenerAsync(year);

        // Both must match {YYYY}-{N} format
        first.Value.Should().MatchRegex(@"^\d{4}-\d+$");
        second.Value.Should().MatchRegex(@"^\d{4}-\d+$");

        // Second must have a higher counter value
        var firstCounter = int.Parse(first.Value.Split('-')[1]);
        var secondCounter = int.Parse(second.Value.Split('-')[1]);
        secondCounter.Should().BeGreaterThan(firstCounter);

        // Year portion must match
        first.Value.Should().StartWith($"{year}-");
        second.Value.Should().StartWith($"{year}-");
    }

    // ------------------------------------------------------------------
    // I-05: incrementar_concurrente_no_duplica
    // 5 concurrent calls must produce distinct NumeroManifiesto values
    // ------------------------------------------------------------------

    [Fact]
    public async Task IncrementarYObtenerAsync_ConcurrentCalls_NoDuplicates()
    {
        const int year = 2024;
        var results = new System.Collections.Concurrent.ConcurrentBag<string>();

        await Parallel.ForEachAsync(Enumerable.Range(0, 5), async (_, ct) =>
        {
            var service = BuildService();
            var result = await service.IncrementarYObtenerAsync(year, ct);
            results.Add(result.Value);
        });

        results.Should().HaveCount(5);
        results.Should().OnlyHaveUniqueItems();
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private NumeradorManifiestoServiceSql BuildService() =>
        new(new FixtureMasterConnectionFactoryForNumerador(_fixture));
}

file sealed class FixtureMasterConnectionFactoryForNumerador(ManifiestoRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create() =>
        new(fixture.MasterConnectionString);
}
