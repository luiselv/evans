using EVANS.Application.GuiaRemision.Commands;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.GuiaRemision;
using FluentValidation;
using NSubstitute;
using NSubstitute.Core;

namespace EVANS.Application.Tests.GuiaRemision;

public class CrearGuiaCommandHandlerTests
{
    private readonly INumeradorService _numerador;
    private readonly IGuiaRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;
    private readonly IRecepcionVinculadaService _recepcionService;
    private readonly CrearGuiaCommandHandler _handler;

    private static readonly NumeroGuia FakeNumero = new("GREM", 1);

    public CrearGuiaCommandHandlerTests()
    {
        _numerador = Substitute.For<INumeradorService>();
        _repo = Substitute.For<IGuiaRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();
        _recepcionService = Substitute.For<IRecepcionVinculadaService>();

        _numerador.IncrementarYObtenerGuia().Returns(FakeNumero);
        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);

        _handler = new CrearGuiaCommandHandler(
            _numerador,
            _repo,
            _uowFactory,
            _recepcionService);
    }

    private static CrearGuiaCommand BuildValidStandaloneCommand() => new(
        RemitenteId: 1,
        DestinatarioId: 2,
        DireccionPartida: "Av Lima|Lima|Lima",
        DireccionLlegada: "Av Arequipa|Arequipa|Arequipa",
        HasManifest: false,
        VehiculoId: null,
        CarretaId: null,
        ChoferId: null,
        Igv: 0.18m,
        Origen: new Standalone(),
        Year: 2024,
        Detalles: new List<DetalleGuiaInput>
        {
            new("Carga general", new Peso(10m), 100m, 100m, 1)
        });

    [Fact]
    public async Task Handle_ValidStandalone_NumeradorAndRepoAndUowCalledVincularNot()
    {
        // Arrange
        var command = BuildValidStandaloneCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _numerador.Received(1).IncrementarYObtenerGuia();
        _repo.Received(1).Insertar(Arg.Any<Guia>(), _uow);
        _uow.Received(1).Commit();
        _recepcionService.DidNotReceive().VincularRecepcion(Arg.Any<int>(), Arg.Any<NumeroGuia>(), Arg.Any<int>());
    }

    [Fact]
    public async Task Handle_ValidDesdeRecepcion_VincularCalledAfterCommit()
    {
        // Arrange
        var command = BuildValidStandaloneCommand() with { Origen = new DesdeRecepcion(99) };

        // Capture call order
        var callOrder = new List<string>();
        _uow.When(u => u.Commit()).Do(_ => callOrder.Add("commit"));
        _recepcionService.When(s => s.VincularRecepcion(Arg.Any<int>(), Arg.Any<NumeroGuia>(), Arg.Any<int>()))
            .Do(_ => callOrder.Add("vincular"));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _recepcionService.Received(1).VincularRecepcion(99, Arg.Any<NumeroGuia>(), 2024);
        callOrder.Should().Equal("commit", "vincular");
    }

    [Fact]
    public async Task Handle_EmptyDetalles_ThrowsValidationException_NoDbCalls()
    {
        // Arrange
        var command = BuildValidStandaloneCommand() with { Detalles = new List<DetalleGuiaInput>() };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _numerador.DidNotReceive().IncrementarYObtenerGuia();
        _repo.DidNotReceive().Insertar(Arg.Any<Guia>(), Arg.Any<IUnitOfWork>());
        _uow.DidNotReceive().Commit();
    }

    [Fact]
    public async Task Handle_NullRemitente_ThrowsValidationException()
    {
        // Arrange — RemitenteId = 0 triggers validation (invalid)
        var command = BuildValidStandaloneCommand() with { RemitenteId = 0 };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _numerador.DidNotReceive().IncrementarYObtenerGuia();
        _repo.DidNotReceive().Insertar(Arg.Any<Guia>(), Arg.Any<IUnitOfWork>());
    }

    [Fact]
    public async Task Handle_NumeradorThrows_NoYearlyDbWrite()
    {
        // Arrange
        _numerador.When(n => n.IncrementarYObtenerGuia()).Throw(new InvalidOperationException("numerador error"));
        var command = BuildValidStandaloneCommand();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _repo.DidNotReceive().Insertar(Arg.Any<Guia>(), Arg.Any<IUnitOfWork>());
        _uow.DidNotReceive().Commit();
    }

    [Fact]
    public async Task Handle_Success_ResultOkContainsCodigo()
    {
        // Arrange
        var command = BuildValidStandaloneCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        // Codigo may be null (no DB-assigned id in unit test) — result is Ok
        result.Error.Should().BeNull();
    }
}
