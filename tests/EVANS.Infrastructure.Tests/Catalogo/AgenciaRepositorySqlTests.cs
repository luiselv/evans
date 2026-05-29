using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;

namespace EVANS.Infrastructure.Tests.Catalogo;

[Collection("GuiaRepository")]
public sealed class AgenciaRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public AgenciaRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetByIdAsync_MapsAgencia()
    {
        var repo = new AgenciaRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var agencia = await repo.GetByIdAsync(1, CancellationToken.None);

        agencia.Should().NotBeNull();
        agencia!.Codigo.Should().Be(1);
        agencia.Direccion.Should().Be("Agencia Central");
        agencia.DestinoCodigo.Should().Be(1);
    }

    [Fact]
    public async Task ListActiveAsync_ReturnsOnlyActiveAgencias()
    {
        var repo = new AgenciaRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var agencias = await repo.ListActiveAsync(CancellationToken.None);

        agencias.Should().Contain(a => a.Codigo == 1 && a.Direccion == "Agencia Central");
        agencias.Should().OnlyContain(a => a.EstadoCodigo == 1);
    }
}
