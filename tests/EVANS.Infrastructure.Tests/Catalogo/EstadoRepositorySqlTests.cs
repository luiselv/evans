using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;

namespace EVANS.Infrastructure.Tests.Catalogo;

[Collection("GuiaRepository")]
public sealed class EstadoRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public EstadoRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetByIdAsync_MapsDescripcionFromEstaDescripcion()
    {
        var repo = new EstadoRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var estado = await repo.GetByIdAsync(1, CancellationToken.None);

        estado.Should().NotBeNull();
        estado!.Codigo.Should().Be(1);
        estado.Descripcion.Should().Be("Activo");
    }

    [Fact]
    public async Task ListAsync_ReturnsSeededEstados()
    {
        var repo = new EstadoRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var estados = await repo.ListAsync(CancellationToken.None);

        estados.Should().Contain(e => e.Codigo == 1 && e.Descripcion == "Activo");
    }
}
