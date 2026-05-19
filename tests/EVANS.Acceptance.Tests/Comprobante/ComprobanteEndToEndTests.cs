using EVANS.Application.Comprobante.Commands;
using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.DependencyInjection;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Shared.Ports;
using EVANS.Domain.Comprobante;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace EVANS.Acceptance.Tests.Comprobante;

/// <summary>
/// E2E acceptance tests that exercise the full Application layer (handlers, validators,
/// domain) using NSubstitute doubles for all infrastructure ports.
/// Pattern mirrors GuiaRemision acceptance tests.
/// </summary>
public class ComprobanteEndToEndTests
{
    private const int TestClienteCodigo = 1;
    private const int TestYear = 2024;

    private static (
        IMediator mediator,
        IComprobanteRepository repo,
        INumeradorComprobanteService numerador,
        IGuiaVinculadaService guiaVinculada,
        IParametrosService parametros,
        IUnitOfWork uow)
        BuildMediator()
    {
        var services = new ServiceCollection();
        services.AddEvansApplication();

        var repo = Substitute.For<IComprobanteRepository>();
        var numerador = Substitute.For<INumeradorComprobanteService>();
        var uowFactory = Substitute.For<IUnitOfWorkFactory>();
        var uow = Substitute.For<IUnitOfWork>();
        var guiaVinculada = Substitute.For<IGuiaVinculadaService>();
        var parametros = Substitute.For<IParametrosService>();

        // Numerador returns valid NumeroComprobante values (async)
        numerador.IncrementarYObtenerAsync(TipoComprobante.Boleta, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new NumeroComprobante("B001", "000001")));
        numerador.IncrementarYObtenerAsync(TipoComprobante.Factura, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new NumeroComprobante("F001", "000001")));

        uowFactory.Create(Arg.Any<int>()).Returns(uow);
        repo.Insertar(Arg.Any<Domain.Comprobante.Comprobante>(), Arg.Any<IUnitOfWork>())
            .Returns(42);

        // IGV rate 18% (async)
        parametros.ObtenerIgvRateAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(0.18m));

        // GuiaVinculada — best-effort, returns true by default
        guiaVinculada.VincularComprobanteAsync(
                Arg.Any<string>(), Arg.Any<NumeroComprobante>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        services.AddSingleton(repo);
        services.AddSingleton(numerador);
        services.AddSingleton(uowFactory);
        services.AddSingleton(guiaVinculada);
        services.AddSingleton(parametros);

        var sp = services.BuildServiceProvider();
        return (sp.GetRequiredService<IMediator>(), repo, numerador, guiaVinculada, parametros, uow);
    }

    // ----------------------------------------------------------------
    // TASK-038 test 1: CrearBoleta_E2E
    // ----------------------------------------------------------------

    [Fact]
    public async Task CrearBoleta_E2E_PersistsBoletaWithZeroIgvAndReturnsOk()
    {
        var (mediator, repo, numerador, _, _, uow) = BuildMediator();

        var cmd = new CrearComprobanteCommand(
            Tipo: TipoComprobante.Boleta,
            ClienteCodigo: TestClienteCodigo,
            RucODni: "12345678",
            Direccion: "Av. Lima 123",
            Detalles:
            [
                new DetalleComprobanteInput(1, "Carga general", 100m, 50m),
                new DetalleComprobanteInput(2, "Carga frágil",  80m,  70m)
            ],
            GuiaRef: null,
            Year: TestYear);

        var result = await mediator.Send(cmd);

        // IsSuccess — boleta created
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);

        // Numerador invoked for Boleta
        await numerador.Received(1).IncrementarYObtenerAsync(TipoComprobante.Boleta, Arg.Any<CancellationToken>());

        // Repository persisted the aggregate
        repo.Received(1).Insertar(
            Arg.Is<Domain.Comprobante.Comprobante>(c =>
                c.Tipo == TipoComprobante.Boleta &&
                c.IGV == 0m &&
                c.ValorVenta == 0m &&
                c.Detalles.Count == 2),
            Arg.Any<IUnitOfWork>());

        // UoW committed once
        uow.Received(1).Commit();
    }

    // ----------------------------------------------------------------
    // TASK-038 test 2: CrearFactura_E2E
    // ----------------------------------------------------------------

    [Fact]
    public async Task CrearFactura_E2E_PersistsFacturaWithCorrectIgvAndReturnsOk()
    {
        var (mediator, repo, numerador, _, _, uow) = BuildMediator();

        // Total = 118; IGV = 118 * 0.18 / 1.18 = 18.00; ValorVenta = 100.00
        var cmd = new CrearComprobanteCommand(
            Tipo: TipoComprobante.Factura,
            ClienteCodigo: TestClienteCodigo,
            RucODni: "20111111111",
            Direccion: "Av. Industrial 456",
            Detalles:
            [
                new DetalleComprobanteInput(1, "Servicio de transporte", 118m, 118m)
            ],
            GuiaRef: null,
            Year: TestYear);

        var result = await mediator.Send(cmd);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);

        await numerador.Received(1).IncrementarYObtenerAsync(TipoComprobante.Factura, Arg.Any<CancellationToken>());

        // IGV ≈ 18.00 and ValorVenta ≈ 100.00 for Total = 118
        repo.Received(1).Insertar(
            Arg.Is<Domain.Comprobante.Comprobante>(c =>
                c.Tipo == TipoComprobante.Factura &&
                c.IGV == 18.00m &&
                c.ValorVenta == 100.00m &&
                c.Total == 118.00m),
            Arg.Any<IUnitOfWork>());

        uow.Received(1).Commit();
    }

    // ----------------------------------------------------------------
    // TASK-038 test 3: CrearFacturaDesdeGuia_E2E
    // ----------------------------------------------------------------

    [Fact]
    public async Task CrearFacturaDesdeGuia_E2E_VincularComprobanteCalledAfterCommit()
    {
        var (mediator, repo, numerador, guiaVinculada, _, uow) = BuildMediator();

        var cmd = new CrearComprobanteCommand(
            Tipo: TipoComprobante.Factura,
            ClienteCodigo: TestClienteCodigo,
            RucODni: "20222222222",
            Direccion: "Av. Arequipa 789",
            Detalles:
            [
                new DetalleComprobanteInput(1, "Transporte especial", 236m, 236m)
            ],
            GuiaRef: "GR01000001",
            Year: TestYear);

        var result = await mediator.Send(cmd);

        result.IsSuccess.Should().BeTrue();

        // GuiaVinculadaService must be called exactly once (post-commit best-effort)
        await guiaVinculada.Received(1).VincularComprobanteAsync(
            "GR01000001",
            Arg.Is<NumeroComprobante>(n => n.Serie == "F001"),
            TestYear,
            Arg.Any<CancellationToken>());

        uow.Received(1).Commit();
    }

    // ----------------------------------------------------------------
    // TASK-038 test 4: BuscarComprobantes_E2E
    // ----------------------------------------------------------------

    [Fact]
    public async Task BuscarComprobantes_E2E_ReturnsMatchingResultsForDateRange()
    {
        var (mediator, repo, _, _, _, _) = BuildMediator();

        // Stub Buscar to return 2 items
        var desde = new DateTime(2024, 1, 1);
        var hasta = new DateTime(2024, 12, 31);

        repo.Buscar(Arg.Any<BuscarComprobantesFiltro>())
            .Returns(new List<ComprobanteResumenDto>
            {
                new(1, "B001-000001", TipoComprobante.Boleta,
                    new DateTime(2024, 6, 1), 1, 50m, false),
                new(2, "F001-000001", TipoComprobante.Factura,
                    new DateTime(2024, 7, 1), 1, 118m, false)
            }.AsReadOnly());

        var query = new Application.Comprobante.Queries.BuscarComprobantesQuery(
            Desde: desde,
            Hasta: hasta,
            ClienteCodigo: null,
            Tipo: null,
            SoloImpreso: null,
            Year: TestYear);

        var results = await mediator.Send(query);

        results.Should().HaveCount(2);
        results.Should().Contain(r => r.Tipo == TipoComprobante.Boleta);
        results.Should().Contain(r => r.Tipo == TipoComprobante.Factura);

        repo.Received(1).Buscar(
            Arg.Is<BuscarComprobantesFiltro>(f =>
                f.Desde == desde &&
                f.Hasta == hasta));
    }
}
