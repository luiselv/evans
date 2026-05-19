using EVANS.Infrastructure.Sql.GuiaRemision;

namespace EVANS.Infrastructure.Tests.GuiaRemision;

[Collection("GuiaRepository")]
public class NumeradorServiceSqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public NumeradorServiceSqlTests(GuiaRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // (a) Sequential calls return strictly increasing NumeroGuia values
    // ------------------------------------------------------------------

    [Fact]
    public void Sequential_calls_return_strictly_increasing_NumeroGuia_values()
    {
        var service = new NumeradorServiceSql(new FixtureMasterConnectionFactory(_fixture));

        var first = service.IncrementarYObtenerGuia();
        var second = service.IncrementarYObtenerGuia();
        var third = service.IncrementarYObtenerGuia();

        second.Numero.Should().BeGreaterThan(first.Numero);
        third.Numero.Should().BeGreaterThan(second.Numero);
    }

    // ------------------------------------------------------------------
    // (b) Returned NumeroGuia.Serie is non-empty
    // ------------------------------------------------------------------

    [Fact]
    public void Returned_NumeroGuia_Serie_is_non_empty()
    {
        var service = new NumeradorServiceSql(new FixtureMasterConnectionFactory(_fixture));

        var result = service.IncrementarYObtenerGuia();

        result.Serie.Should().NotBeNullOrWhiteSpace();
    }
}

file sealed class FixtureMasterConnectionFactory(GuiaRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create() =>
        new(fixture.MasterConnectionString);
}
