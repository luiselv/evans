using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;
using EVANS.Domain.Catalogo;

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

    [Fact]
    public async Task ListAllAsync_ReturnsActiveAndInactiveCarretasOrderedByCodigo()
    {
        var repo = new CarretaRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);

        var createdCodigo = await repo.AddAsync(
            Carreta.Crear("ABC-123", "Scania", "CERT", 1, CatalogoEstado.Activo),
            CancellationToken.None);
        await repo.DeactivateAsync(1, CancellationToken.None);

        var carretas = await repo.ListAllAsync(CancellationToken.None);

        carretas.Should().Contain(c => c.Codigo == 1 && c.EstadoCodigo == CatalogoEstado.Inactivo);
        carretas.Should().Contain(c => c.Codigo == createdCodigo && c.EstadoCodigo == CatalogoEstado.Activo);
        carretas.Select(c => c.Codigo).Should().BeInAscendingOrder();
    }
}
