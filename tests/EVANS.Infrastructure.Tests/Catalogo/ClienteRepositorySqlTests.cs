using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;
using EVANS.Domain.Catalogo;

namespace EVANS.Infrastructure.Tests.Catalogo;

[Collection("GuiaRepository")]
public sealed class ClienteRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public ClienteRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetByIdAsync_MapsClienteWithDirecciones()
    {
        var repo = new ClienteRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureClienteDireccionAsync(_fixture.MasterConnectionString);

        var cliente = await repo.GetByIdAsync(1, CancellationToken.None);

        cliente.Should().NotBeNull();
        cliente!.Codigo.Should().Be(1);
        cliente.RazonSocial.Should().Be("Remitente Test SA");
        cliente.Direcciones.Should().ContainSingle(d =>
            d.Calle == "Av Test 123" && d.Ciudad == "Lima" && d.Provincia == "Lima");
    }

    [Fact]
    public async Task ListAsync_ReturnsSeededClientes()
    {
        var repo = new ClienteRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureClienteDireccionAsync(_fixture.MasterConnectionString);

        var clientes = await repo.ListAsync(CancellationToken.None);

        clientes.Should().Contain(c => c.Codigo == 1 && c.RazonSocial == "Remitente Test SA");
    }

    [Fact]
    public async Task AddAsync_PersistsLegacyOptionalFieldsWithoutDireccion()
    {
        var repo = new ClienteRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        var cliente = Cliente.Crear(
            "CLIENTE SIN DIRECCION", 1, "20123456789", 11, "555", "123", "ops@test.local", "ANA", []);

        var codigo = await repo.AddAsync(cliente, CancellationToken.None);
        var persisted = await repo.GetByIdAsync(codigo, CancellationToken.None);

        persisted.Should().NotBeNull();
        persisted!.Fax.Should().Be("123");
        persisted.Representante.Should().Be("ANA");
        persisted.Direcciones.Should().BeEmpty();
    }
}
