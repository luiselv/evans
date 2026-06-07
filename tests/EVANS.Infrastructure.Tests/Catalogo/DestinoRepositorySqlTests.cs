using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;

namespace EVANS.Infrastructure.Tests.Catalogo;

[Collection("GuiaRepository")]
public sealed class DestinoRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public DestinoRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ListActiveAsync_ReturnsOnlyActiveDestinos()
    {
        var repo = new DestinoRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var destinos = await repo.ListActiveAsync(CancellationToken.None);

        destinos.Should().Contain(d => d.Codigo == 1 && d.Descripcion == "Lima");
        destinos.Should().OnlyContain(d => d.EstadoCodigo == 1);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsInactiveDestinosOrderedByCodigo()
    {
        var repo = new DestinoRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);
        await repo.DeactivateAsync(1, CancellationToken.None);

        var destinos = await repo.ListAllAsync(CancellationToken.None);

        destinos.Should().Contain(d => d.Codigo == 1 && d.EstadoCodigo == 2);
        destinos.Select(d => d.Codigo).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task DeactivateAsync_SetsEstadoCodigoInactive()
    {
        var repo = new DestinoRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);

        await repo.DeactivateAsync(1, CancellationToken.None);

        var destino = await repo.GetByIdAsync(1, CancellationToken.None);
        destino.Should().NotBeNull();
        destino!.EstadoCodigo.Should().Be(2);
    }
}
