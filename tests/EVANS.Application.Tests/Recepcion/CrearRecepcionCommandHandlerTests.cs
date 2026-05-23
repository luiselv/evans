using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Recepcion.Commands;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using EVANS.Domain.Recepcion;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Agg = EVANS.Domain.Recepcion.Recepcion;

namespace EVANS.Application.Tests.Recepcion;

public class CrearRecepcionCommandHandlerTests
{
    private readonly IRecepcionRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;
    private readonly CrearRecepcionCommandHandler _handler;

    public CrearRecepcionCommandHandlerTests()
    {
        _repo = Substitute.For<IRecepcionRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();

        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);
        _repo.CrearAsync(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult(42));

        _handler = new CrearRecepcionCommandHandler(_repo, _uowFactory);
    }

    private static IReadOnlyList<DetalleRecepcionInput> OneDetalle() =>
        new[] { new DetalleRecepcionInput(1m, "Carga", 5m, "KG", 100m, "GR", "001") };

    private static CrearRecepcionCommand ValidCommand(
        IReadOnlyList<DetalleRecepcionInput>? detalles = null,
        bool aplicarIgv = false,
        decimal tasaIgv = 0.18m) =>
        new(
            FechaEmision: DateTime.Today,
            RemitenteId: 1,
            TipoDirPartida: TipoDireccion.Agencia,
            DireccionPartida: "Agencia Lima",
            DestinatarioId: 2,
            TipoDirDestino: TipoDireccion.DireccionCliente,
            DireccionDestino: "Av Arequipa 123",
            DestinoId: 1,
            EstadoId: 1,
            Bultos: null,
            PesoTotal: null,
            CostoTotal: 100m,
            Observacion: null,
            UsuarioId: 1,
            Detalles: detalles ?? OneDetalle(),
            Year: 2026,
            AplicarIgv: aplicarIgv,
            TasaIgv: tasaIgv);

    // A-01: happy path — repo.CrearAsync called, returns Codigo
    [Fact]
    public async Task Handle_HappyPath_RepoCalled_ReturnsCodigo()
    {
        var result = await _handler.Handle(ValidCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        await _repo.Received(1).CrearAsync(Arg.Any<Agg>(), _uow, Arg.Any<CancellationToken>());
        _uow.Received(1).Commit();
    }

    // A-02: AplicarIgv = true — detail costo adjusted before persist
    [Fact]
    public async Task Handle_AplicarIgvTrue_DetalleAjustadoAntesDeGuardar()
    {
        var detalles = new[] { new DetalleRecepcionInput(1m, "Item", 0m, "UND", 118m, "GR", "001") };
        Agg? capturedAggregate = null;

        _repo.CrearAsync(Arg.Do<Agg>(a => capturedAggregate = a), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult(1));

        await _handler.Handle(ValidCommand(detalles, aplicarIgv: true, tasaIgv: 0.18m), CancellationToken.None);

        capturedAggregate.Should().NotBeNull();
        capturedAggregate!.Detalles[0].Costo.Should().Be(100m);
    }

    // A-10: validation rejects DestinoId = 0 — repo never called
    [Fact]
    public async Task Handle_DestinoIdCero_ThrowsValidationException_RepoNeverCalled()
    {
        var cmd = ValidCommand() with { DestinoId = 0 };

        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(cmd, CancellationToken.None));

        await _repo.DidNotReceive().CrearAsync(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>());
    }
}
