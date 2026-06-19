using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Tests.GuiaRemision;
using EVANS.Domain.Catalogo;

namespace EVANS.Infrastructure.Tests.Catalogo;

[Collection("GuiaRepository")]
public sealed class VehiculoRepositorySqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public VehiculoRepositorySqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ListActiveAsync_ReturnsOnlyActiveVehiculos()
    {
        var repo = new VehiculoRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));

        var vehiculos = await repo.ListActiveAsync(CancellationToken.None);

        vehiculos.Should().Contain(v => v.Codigo == 1 && v.Placa == "ABC-123");
        vehiculos.Should().OnlyContain(v => v.EstadoCodigo == 1);
    }

    [Fact]
    public async Task DeactivateAsync_SetsEstadoCodigoInactive()
    {
        var repo = new VehiculoRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);

        await repo.DeactivateAsync(1, CancellationToken.None);

        var vehiculo = await repo.GetByIdAsync(1, CancellationToken.None);
        vehiculo.Should().NotBeNull();
        vehiculo!.EstadoCodigo.Should().Be(2);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsActiveAndInactiveVehiculosOrderedByCodigo()
    {
        var repo = new VehiculoRepositorySql(new FixedMasterConnectionFactory(_fixture.MasterConnectionString));
        await CatalogoSeed.EnsureInactiveEstadoAsync(_fixture.MasterConnectionString);

        var createdCodigo = await repo.AddAsync(
            Vehiculo.Crear("Scania", "XYZ-789", "C3", "CERT", 1, CatalogoEstado.Activo),
            CancellationToken.None);
        await repo.DeactivateAsync(1, CancellationToken.None);

        var vehiculos = await repo.ListAllAsync(CancellationToken.None);

        vehiculos.Should().Contain(v => v.Codigo == 1 && v.EstadoCodigo == CatalogoEstado.Inactivo);
        vehiculos.Should().Contain(v => v.Codigo == createdCodigo && v.EstadoCodigo == CatalogoEstado.Activo);
        vehiculos.Select(v => v.Codigo).Should().BeInAscendingOrder();
    }
}
