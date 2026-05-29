using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;

namespace EVANS.Infrastructure.Tests.Catalogo;

[Collection("GuiaRepository")]
public sealed class CarretaRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public CarretaRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ListActiveAsync_ReturnsOnlyActiveCarretas()
    {
        var repo = new CarretaRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var carretas = await repo.ListActiveAsync(CancellationToken.None);

        carretas.Should().Contain(c => c.Codigo == 1 && c.Placa == "XYZ-789");
        carretas.Should().OnlyContain(c => c.EstadoCodigo == 1);
    }

    [Fact]
    public async Task DeactivateAsync_SetsEstadoCodigoInactive()
    {
        var repo = new CarretaRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);

        await repo.DeactivateAsync(1, CancellationToken.None);

        var carreta = await repo.GetByIdAsync(1, CancellationToken.None);
        carreta.Should().NotBeNull();
        carreta!.EstadoCodigo.Should().Be(2);
    }
}
