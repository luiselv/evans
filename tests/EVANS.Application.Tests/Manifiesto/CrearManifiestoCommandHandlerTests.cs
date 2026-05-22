using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Manifiesto.Commands;
using EVANS.Application.Manifiesto.Handlers;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Domain.Manifiesto;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace EVANS.Application.Tests.Manifiesto;

public class CrearManifiestoCommandHandlerTests
{
    private readonly IManifiestoRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;
    private readonly INumeradorManifiestoService _numerador;
    private readonly IGuiasManifiestoService _guiasService;
    private readonly ILogger<CrearManifiestoCommandHandler> _logger;
    private readonly CrearManifiestoCommandHandler _handler;

    private static readonly NumeroManifiesto FakeNumero = new("2024-1");

    public CrearManifiestoCommandHandlerTests()
    {
        _repo = Substitute.For<IManifiestoRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();
        _numerador = Substitute.For<INumeradorManifiestoService>();
        _guiasService = Substitute.For<IGuiasManifiestoService>();
        _logger = Substitute.For<ILogger<CrearManifiestoCommandHandler>>();

        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);
        _numerador.IncrementarYObtenerAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                  .Returns(Task.FromResult(FakeNumero));
        _repo.InsertarAsync(Arg.Any<EVANS.Domain.Manifiesto.Manifiesto>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult(99));
        _guiasService.MarcarGuiasEnviadasAsync(
                Arg.Any<IReadOnlyList<int>>(),
                Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<CarrierInfo>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GuiasMarcadoResult(2, Array.Empty<int>())));

        _handler = new CrearManifiestoCommandHandler(
            _repo, _uowFactory, _numerador, _guiasService, _logger);
    }

    private static CrearManifiestoCommand BuildCommand(IReadOnlyList<int>? guiaIds = null) => new(
        Fecha: DateTime.Today,
        TransportistaCodigo: 5,
        VehiculoCodigo: 3,
        CarretaCodigo: null,
        ChoferCodigo: 7,
        Importe: 100m,
        Peso: 50m,
        EstadoCodigo: 1,
        UsuarioCodigo: 2,
        GuiaIds: guiaIds ?? new List<int> { 1, 2 },
        Year: 2024);

    // A-01: happy path — numerador called, repo.InsertarAsync called, returns Ok(codigo)
    [Fact]
    public async Task Handle_HappyPath_NumeradorYRepositorioLlamados()
    {
        var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(99);
        await _numerador.Received(1).IncrementarYObtenerAsync(2024, Arg.Any<CancellationToken>());
        await _repo.Received(1).InsertarAsync(
            Arg.Any<EVANS.Domain.Manifiesto.Manifiesto>(),
            _uow,
            Arg.Any<CancellationToken>());
        _uow.Received(1).Commit();
    }

    // A-02: post-commit marca guias enviadas
    [Fact]
    public async Task Handle_PostCommit_MarcaGuiasEnviadas()
    {
        var callOrder = new List<string>();
        _uow.When(u => u.Commit()).Do(_ => callOrder.Add("commit"));
        _guiasService.MarcarGuiasEnviadasAsync(
                Arg.Any<IReadOnlyList<int>>(),
                Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<CarrierInfo>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                callOrder.Add("marcarEnviadas");
                return Task.FromResult(new GuiasMarcadoResult(2, Array.Empty<int>()));
            });

        await _handler.Handle(BuildCommand(), CancellationToken.None);

        callOrder.Should().Equal("commit", "marcarEnviadas");
        await _guiasService.Received(1).MarcarGuiasEnviadasAsync(
            Arg.Is<IReadOnlyList<int>>(ids => ids.Count == 2),
            FakeNumero.Value,
            Arg.Any<DateTime>(),
            Arg.Any<CarrierInfo>(),
            2024,
            Arg.Any<CancellationToken>());
    }

    // A-03: guias service throws → handler still returns Ok (best-effort)
    [Fact]
    public async Task Handle_GuiasServiceThrows_HandlerStillReturnsOk()
    {
        _guiasService.MarcarGuiasEnviadasAsync(
                Arg.Any<IReadOnlyList<int>>(),
                Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<CarrierInfo>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Throws(new InvalidOperationException("Guias service error"));

        var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    // A-04: guias parcialmente no encontradas → solo loguea, retorna Ok
    [Fact]
    public async Task Handle_GuiasParcialmenteNoEncontradas_SoloLoguea()
    {
        _guiasService.MarcarGuiasEnviadasAsync(
                Arg.Any<IReadOnlyList<int>>(),
                Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<CarrierInfo>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GuiasMarcadoResult(1, new List<int> { 5 })));

        var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    // A-12: validacion falla antes del numerador
    [Fact]
    public async Task Handle_GuiasVacias_ThrowsValidationException_NumeradorNoLlamado()
    {
        var command = BuildCommand(guiaIds: new List<int>());

        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        await _numerador.DidNotReceive().IncrementarYObtenerAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }
}
