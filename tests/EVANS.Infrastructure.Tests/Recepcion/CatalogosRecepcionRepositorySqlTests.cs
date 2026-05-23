using EVANS.Infrastructure.Sql.Recepcion;
using FluentAssertions;
using Xunit;

namespace EVANS.Infrastructure.Tests.Recepcion;

[Collection("RecepcionRepository")]
public class CatalogosRecepcionRepositorySqlTests : IAsyncLifetime
{
    private readonly RecepcionRepositoryFixture _fixture;

    public CatalogosRecepcionRepositorySqlTests(RecepcionRepositoryFixture fixture) => _fixture = fixture;
    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // I-08: ListarClientesAsync returns non-empty list
    [Fact]
    public async Task ListarClientes_RetornaListaNoVacia()
    {
        var masterFactory = new FixedMasterConnectionFactory(_fixture.MasterConnectionString);
        var repo = new CatalogosRecepcionRepositorySql(masterFactory);

        var result = await repo.ListarClientesAsync(CancellationToken.None);

        result.Should().NotBeEmpty();
        result.Should().AllSatisfy(c => c.Codigo.Should().BeGreaterThan(0));
        result.Should().AllSatisfy(c => c.Nombre.Should().NotBeNull());
    }

    // ListarEstadosAsync returns seeded estado
    [Fact]
    public async Task ListarEstados_RetornaEstadoSeed()
    {
        var masterFactory = new FixedMasterConnectionFactory(_fixture.MasterConnectionString);
        var repo = new CatalogosRecepcionRepositorySql(masterFactory);

        var result = await repo.ListarEstadosAsync(CancellationToken.None);

        result.Should().NotBeEmpty();
        result.Should().Contain(e => e.Codigo == 1);
    }
}
