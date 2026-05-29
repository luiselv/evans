using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;

namespace EVANS.Infrastructure.Tests.Catalogo;

[Collection("GuiaRepository")]
public sealed class TipoIdentificacionRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public TipoIdentificacionRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetByIdAsync_MapsRucWithRequiredLength()
    {
        var repo = new TipoIdentificacionRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var tipo = await repo.GetByIdAsync(1, CancellationToken.None);

        tipo.Should().NotBeNull();
        tipo!.Codigo.Should().Be(1);
        tipo.Descripcion.Should().Be("RUC");
        tipo.LongitudRequerida.Should().Be(11);
    }

    [Fact]
    public async Task ListAsync_ReturnsSeededTiposIdentificacion()
    {
        var repo = new TipoIdentificacionRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var tipos = await repo.ListAsync(CancellationToken.None);

        tipos.Should().Contain(t => t.Codigo == 1 && t.Descripcion == "RUC" && t.LongitudRequerida == 11);
    }
}
