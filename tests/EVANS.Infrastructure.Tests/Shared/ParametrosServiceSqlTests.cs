using EVANS.Infrastructure.Sql.Shared;
using EVANS.Infrastructure.Tests.GuiaRemision;
using EVANS.Application.Shared.DTOs;

namespace EVANS.Infrastructure.Tests.Shared;

[Collection("GuiaRepository")]
public sealed class ParametrosServiceSqlTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public ParametrosServiceSqlTests(GuiaRepositoryFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ObtenerParametrosAsync_MapsLegacyParametrosRow()
    {
        var service = new ParametrosServiceSql(new FixtureMasterConnectionFactory(_fixture));

        var parametros = await service.ObtenerParametrosAsync(CancellationToken.None);

        parametros.IgvRate.Should().Be(0.18m);
        parametros.FacturaSerie.Should().Be("F001");
        parametros.BoletaSerie.Should().Be("B001");
        parametros.GuiaRemisionSerie.Should().Be("GR01");
    }

    [Fact]
    public async Task ObtenerIgvRateAsync_ReusesParametrosSnapshot()
    {
        var service = new ParametrosServiceSql(new FixtureMasterConnectionFactory(_fixture));

        var igv = await service.ObtenerIgvRateAsync(CancellationToken.None);

        igv.Should().Be(0.18m);
    }

    [Fact]
    public async Task ActualizarParametrosAsync_UpdatesLegacyParametrosRow()
    {
        var service = new ParametrosServiceSql(new FixtureMasterConnectionFactory(_fixture));
        var parametros = new ParametrosDto(
            IgvRate: 0.19m,
            FacturaSerie: "F002",
            FacturaNro1: "000123",
            FacturaNro2: "0",
            BoletaSerie: "B002",
            BoletaNro1: "000456",
            BoletaNro2: "0",
            GuiaRemisionSerie: "GR02",
            GuiaRemisionNro1: "000789",
            GuiaRemisionNro2: "0",
            Manifiesto: "M002",
            Remitente: "EVANS SAC",
            EmailRemitente: "evans@example.com",
            PassRemitente: "secret",
            Smtp: "smtp.example.com",
            Puerto: 587);

        await service.ActualizarParametrosAsync(parametros, CancellationToken.None);

        var updated = await service.ObtenerParametrosAsync(CancellationToken.None);
        updated.IgvRate.Should().Be(0.19m);
        updated.FacturaSerie.Should().Be("F002");
        updated.FacturaNro1.Should().Be("000123");
        updated.BoletaSerie.Should().Be("B002");
        updated.BoletaNro1.Should().Be("000456");
        updated.GuiaRemisionSerie.Should().Be("GR02");
        updated.GuiaRemisionNro1.Should().Be("000789");
        updated.Manifiesto.Should().Be("M002");
        updated.EmailRemitente.Should().Be("evans@example.com");
        updated.Puerto.Should().Be(587);
    }
}

file sealed class FixtureMasterConnectionFactory(GuiaRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create() =>
        new(fixture.MasterConnectionString);
}
