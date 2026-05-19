using EVANS.Application.Common;
using EVANS.Application.Comprobante.Commands;
using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Handlers;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Shared.Ports;
using EVANS.Domain.Comprobante;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Agg = EVANS.Domain.Comprobante.Comprobante;

namespace EVANS.Application.Tests.Comprobante;

public class CrearComprobanteHandlerTests
{
    private readonly INumeradorComprobanteService _numerador;
    private readonly IComprobanteRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;
    private readonly IGuiaVinculadaService _guiaService;
    private readonly IParametrosService _parametros;
    private readonly CrearComprobanteCommandHandler _handler;

    private static readonly NumeroComprobante FakeNumeroFactura = new("F001", "000001");
    private static readonly NumeroComprobante FakeNumeroBoleta = new("B001", "000001");

    public CrearComprobanteHandlerTests()
    {
        _numerador = Substitute.For<INumeradorComprobanteService>();
        _repo = Substitute.For<IComprobanteRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();
        _guiaService = Substitute.For<IGuiaVinculadaService>();
        _parametros = Substitute.For<IParametrosService>();

        _numerador.IncrementarYObtenerAsync(TipoComprobante.Factura, Arg.Any<CancellationToken>())
                  .Returns(Task.FromResult(FakeNumeroFactura));
        _numerador.IncrementarYObtenerAsync(TipoComprobante.Boleta, Arg.Any<CancellationToken>())
                  .Returns(Task.FromResult(FakeNumeroBoleta));
        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);
        _parametros.ObtenerIgvRateAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(0.18m));
        _repo.Insertar(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>()).Returns(42);

        _handler = new CrearComprobanteCommandHandler(
            _numerador,
            _repo,
            _uowFactory,
            _guiaService,
            _parametros);
    }

    private static CrearComprobanteCommand BuildFacturaCommand(string? guiaRef = null) => new(
        Tipo: TipoComprobante.Factura,
        ClienteCodigo: 1,
        RucODni: "20123456789",
        Direccion: "Av Lima 123",
        Detalles: new List<DetalleComprobanteInput>
        {
            new(1, "Flete Lima-Arequipa", 100m, 118m)
        },
        GuiaRef: guiaRef,
        Year: 2024);

    private static CrearComprobanteCommand BuildBoletaCommand() => new(
        Tipo: TipoComprobante.Boleta,
        ClienteCodigo: 1,
        RucODni: "12345678",
        Direccion: "Av Lima 123",
        Detalles: new List<DetalleComprobanteInput>
        {
            new(1, "Flete Lima-Arequipa", 100m, 118m)
        },
        GuiaRef: null,
        Year: 2024);

    // --- Factura: numerador, repo, uow called ---

    [Fact]
    public async Task Handle_CrearFactura_NumeradorRepoAndUowCalled()
    {
        var result = await _handler.Handle(BuildFacturaCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _numerador.Received(1).IncrementarYObtenerAsync(TipoComprobante.Factura, Arg.Any<CancellationToken>());
        _repo.Received(1).Insertar(Arg.Any<Agg>(), _uow);
        _uow.Received(1).Commit();
    }

    [Fact]
    public async Task Handle_CrearFactura_ReturnsCodigo()
    {
        var result = await _handler.Handle(BuildFacturaCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    // --- Boleta: ValorVenta=0, IGV=0 in persisted aggregate ---

    [Fact]
    public async Task Handle_CrearBoleta_AggregateHasZeroIgvAndValorVenta()
    {
        Agg? captured = null;
        _repo.When(r => r.Insertar(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>()))
             .Do(ci => captured = ci.Arg<Agg>());

        await _handler.Handle(BuildBoletaCommand(), CancellationToken.None);

        captured.Should().NotBeNull();
        captured!.IGV.Should().Be(0m);
        captured.ValorVenta.Should().Be(0m);
        captured.Tipo.Should().Be(TipoComprobante.Boleta);
    }

    // --- DesdeGuia: GuiaVinculadaService called AFTER UoW.Commit() ---

    [Fact]
    public async Task Handle_DesdeGuia_VincularCalledAfterCommit()
    {
        var callOrder = new List<string>();
        _uow.When(u => u.Commit()).Do(_ => callOrder.Add("commit"));
        _guiaService.VincularComprobanteAsync(Arg.Any<string>(), Arg.Any<NumeroComprobante>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                   .Returns(callInfo => { callOrder.Add("vincular"); return Task.FromResult(true); });

        await _handler.Handle(BuildFacturaCommand(guiaRef: "G001-000001"), CancellationToken.None);

        await _guiaService.Received(1).VincularComprobanteAsync("G001-000001", Arg.Any<NumeroComprobante>(), 2024, Arg.Any<CancellationToken>());
        callOrder.Should().Equal("commit", "vincular");
    }

    // --- GuiaVinculadaService throws → handler still returns success (best-effort) ---

    [Fact]
    public async Task Handle_GuiaServiceThrows_HandlerStillSucceeds()
    {
        _guiaService.VincularComprobanteAsync(Arg.Any<string>(), Arg.Any<NumeroComprobante>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                   .Throws(new InvalidOperationException("Guia service error"));

        var result = await _handler.Handle(BuildFacturaCommand(guiaRef: "G001-000001"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    // --- Standalone: GuiaVinculadaService NOT called ---

    [Fact]
    public async Task Handle_Standalone_VincularNotCalled()
    {
        await _handler.Handle(BuildFacturaCommand(guiaRef: null), CancellationToken.None);

        await _guiaService.DidNotReceive().VincularComprobanteAsync(Arg.Any<string>(), Arg.Any<NumeroComprobante>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
    }

    // --- Validation fails → repository NOT called ---

    [Fact]
    public async Task Handle_EmptyDetalles_ThrowsValidationException_RepoNotCalled()
    {
        var command = BuildFacturaCommand() with
        {
            Detalles = new List<DetalleComprobanteInput>()
        };

        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _repo.DidNotReceive().Insertar(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>());
        _uow.DidNotReceive().Commit();
    }

    [Fact]
    public async Task Handle_InvalidClienteCodigo_ThrowsValidationException()
    {
        var command = BuildFacturaCommand() with { ClienteCodigo = 0 };

        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _repo.DidNotReceive().Insertar(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>());
    }
}
