using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;
using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;

namespace EVANS.Infrastructure.Tests.Catalogo;

[Collection("GuiaRepository")]
public sealed class EmpresaRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public EmpresaRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ListActiveAsync_ReturnsOnlyActiveEmpresas()
    {
        var repo = new EmpresaRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var empresas = await repo.ListActiveAsync(CancellationToken.None);

        empresas.Should().Contain(e => e.Codigo == 1 && e.RazonSocial == "Empresa Test");
        empresas.Should().OnlyContain(e => e.EstadoCodigo == 1);
    }

    [Fact]
    public async Task DeactivateAsync_SetsEstadoCodigoInactiveAndRemovesFromActiveList()
    {
        var repo = new EmpresaRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);

        await repo.DeactivateAsync(1, CancellationToken.None);

        var empresa = await repo.GetByIdAsync(1, CancellationToken.None);
        empresa.Should().NotBeNull();
        empresa!.EstadoCodigo.Should().Be(2);

        var active = await repo.ListActiveAsync(CancellationToken.None);
        active.Should().NotContain(e => e.Codigo == 1);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsActiveAndInactiveEmpresasOrderedByCodigo()
    {
        var repo = new EmpresaRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);
        Ruc.TryCreate("20987654321", out var ruc).Should().BeTrue();

        var createdCodigo = await repo.AddAsync(
            Empresa.Crear("Empresa Activa Dos", null, null, ruc, esPropia: false),
            CancellationToken.None);
        await repo.DeactivateAsync(1, CancellationToken.None);

        var empresas = await repo.ListAllAsync(CancellationToken.None);

        empresas.Should().Contain(e => e.Codigo == 1 && e.EstadoCodigo == CatalogoEstado.Inactivo);
        empresas.Should().Contain(e => e.Codigo == createdCodigo && e.EstadoCodigo == CatalogoEstado.Activo);
        empresas.Select(e => e.Codigo).Should().BeInAscendingOrder();
    }
}
