using EVANS.Infrastructure.Sql.Shared;
using EVANS.Infrastructure.Tests.GuiaRemision;

namespace EVANS.Infrastructure.Tests.Shared;

[Collection("GuiaRepository")]
public sealed class ParametrosServiceSqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public ParametrosServiceSqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ObtenerParametrosAsync_MapsLegacyParametrosRow()
    {
        var service = new ParametrosServiceSql(new FixtureMasterConnectionFactory(_fixture));

        var parametros = await service.ObtenerParametrosAsync(CancellationToken.None);

        parametros.IgvRate.Should().Be(0.18m);
        parametros.FacturaSerie.Should().Be("F001");
        parametros.BoletaSerie.Should().Be("B001");
        parametros.GuiaRemisionSerie.Should().Be("GR01");
    }

    [Fact]
    public async Task ObtenerIgvRateAsync_ReusesParametrosSnapshot()
    {
        var service = new ParametrosServiceSql(new FixtureMasterConnectionFactory(_fixture));

        var igv = await service.ObtenerIgvRateAsync(CancellationToken.None);

        igv.Should().Be(0.18m);
    }
}

file sealed class FixtureMasterConnectionFactory(GuiaRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create() =>
        new(fixture.MasterConnectionString);
}
