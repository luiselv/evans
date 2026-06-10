using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;
using EVANS.Domain.Catalogo;

namespace EVANS.Infrastructure.Tests.Catalogo;

[Collection("GuiaRepository")]
public sealed class ChoferRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public ChoferRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ListActiveAsync_ReturnsOnlyActiveChoferes()
    {
        var repo = new ChoferRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var choferes = await repo.ListActiveAsync(CancellationToken.None);

        choferes.Should().Contain(c => c.Codigo == 1 && c.NombreCompleto == "Juan Perez");
        choferes.Should().OnlyContain(c => c.EstadoCodigo == 1);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsActiveAndInactiveChoferesOrderedByCodigo()
    {
        var repo = new ChoferRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);

        var createdCodigo = await repo.AddAsync(
            Chofer.Crear("Chofer Activo Dos", "Q12345678", null, null, 1),
            CancellationToken.None);
        await repo.DeactivateAsync(1, CancellationToken.None);

        var choferes = await repo.ListAllAsync(CancellationToken.None);

        choferes.Should().Contain(c => c.Codigo == 1 && c.EstadoCodigo == CatalogoEstado.Inactivo);
        choferes.Should().Contain(c => c.Codigo == createdCodigo && c.EstadoCodigo == CatalogoEstado.Activo);
        choferes.Select(c => c.Codigo).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task DeactivateAsync_SetsEstadoCodigoInactive()
    {
        var repo = new ChoferRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);

        await repo.DeactivateAsync(1, CancellationToken.None);

        var chofer = await repo.GetByIdAsync(1, CancellationToken.None);
        chofer.Should().NotBeNull();
        chofer!.EstadoCodigo.Should().Be(2);
    }
}
