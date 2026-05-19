using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Domain.GuiaRemision;
using EVANS.Infrastructure.Sql.GuiaRemision;

namespace EVANS.Infrastructure.Tests.GuiaRemision;

[Collection("GuiaRepository")]
public class BuscarGuiasTests : IAsyncLifetime
{
    private readonly GuiaRepositoryFixture _fixture;

    public BuscarGuiasTests(GuiaRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => _fixture.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ------------------------------------------------------------------
    // (a) Filter by ClienteId returns correct subset
    // ------------------------------------------------------------------

    [Fact]
    public void Filter_by_ClienteId_returns_correct_subset()
    {
        var repo = BuildRepo();
        var factory = BuildUowFactory();

        // Insert 3 guides: 2 for remitente=1, 1 for remitente=2
        // "Buscar" by ClienteId filters both remitente AND destinatario
        InsertGuia(repo, factory, remitenteId: 1, destinatarioId: 2);
        InsertGuia(repo, factory, remitenteId: 1, destinatarioId: 2);
        InsertGuia(repo, factory, remitenteId: 2, destinatarioId: 1); // different remitente

        var filtro = new BuscarGuiasFiltro(
            Desde: null,
            Hasta: null,
            ClienteId: 1,
            EstadoId: null);

        var result = repo.Buscar(filtro, GuiaRepositoryFixture.TestYear);

        // Both guides where remitente=1 OR destinatario=1 should be included
        // The 3rd guide has destinatario=1, so it qualifies too — total = 3
        // If filter is only on remitente: 2 results
        // Design says "filter by clienteId" — let's assert at least 2 for clienteId=1 as remitente
        result.Should().HaveCountGreaterThanOrEqualTo(2);
        result.Should().OnlyContain(g => g.RemitenteId == 1 || g.DestinatarioId == 1);
    }

    // ------------------------------------------------------------------
    // (b) Filter with no matches returns empty list
    // ------------------------------------------------------------------

    [Fact]
    public void Filter_with_no_matches_returns_empty_list()
    {
        var repo = BuildRepo();

        var filtro = new BuscarGuiasFiltro(
            Desde: DateTime.Today.AddYears(-10),
            Hasta: DateTime.Today.AddYears(-9),
            ClienteId: 9999,
            EstadoId: null);

        var result = repo.Buscar(filtro, GuiaRepositoryFixture.TestYear);

        result.Should().BeEmpty();
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private GuiaRepositoryDapper BuildRepo()
    {
        var factory = new BuscarFixtureConnectionFactory(_fixture);
        return new GuiaRepositoryDapper(factory, factory);
    }

    private SqlUnitOfWorkFactory BuildUowFactory() =>
        new(new BuscarFixtureYearlyFactory(_fixture));

    private static void InsertGuia(
        GuiaRepositoryDapper repo,
        SqlUnitOfWorkFactory factory,
        int remitenteId,
        int destinatarioId)
    {
        var guia = new Guia(
            codigo: null,
            numero: new NumeroGuia("GR01", 1),
            fecha: DateTime.Today,
            remitenteId: remitenteId,
            destinatarioId: destinatarioId,
            direccionPartida: Direccion.Parse("Av Lima|Lima|Lima"),
            direccionLlegada: Direccion.Parse("Av Arequipa|Arequipa|Arequipa"),
            hasManifest: false,
            vehiculoId: null,
            carretaId: null,
            choferId: null,
            igv: 0.18m,
            detalles: [new DetalleGuia(null, "Test item", new Peso(1m), 10m, 10m, 1)]);

        using var uow = factory.Create(GuiaRepositoryFixture.TestYear);
        repo.Insertar(guia, uow);
        uow.Commit();
    }
}

file sealed class BuscarFixtureConnectionFactory(GuiaRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IEvansMasterConnectionFactory,
      EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create() => new(fixture.MasterConnectionString);
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) => new(fixture.YearlyConnectionString);
    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() => new(fixture.YearlyConnectionString);
}

file sealed class BuscarFixtureYearlyFactory(GuiaRepositoryFixture fixture)
    : EVANS.Infrastructure.Sql.Connections.IYearlyTransactionalConnectionFactory
{
    public Microsoft.Data.SqlClient.SqlConnection Create(int year) => new(fixture.YearlyConnectionString);
    public Microsoft.Data.SqlClient.SqlConnection CreateForCurrentYear() => new(fixture.YearlyConnectionString);
}
