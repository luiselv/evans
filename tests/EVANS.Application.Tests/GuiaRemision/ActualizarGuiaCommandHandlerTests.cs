using EVANS.Application.GuiaRemision.Commands;
using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.GuiaRemision;
using FluentValidation;
using NSubstitute;

namespace EVANS.Application.Tests.GuiaRemision;

public class ActualizarGuiaCommandHandlerTests
{
    private readonly IGuiaRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;
    private readonly ActualizarGuiaCommandHandler _handler;

    public ActualizarGuiaCommandHandlerTests()
    {
        _repo = Substitute.For<IGuiaRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();

        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);

        _handler = new ActualizarGuiaCommandHandler(_repo, _uowFactory);
    }

    private static ActualizarGuiaCommand BuildValidCommand() => new(
        Codigo: 5,
        RemitenteId: 1,
        DestinatarioId: 2,
        DireccionPartida: "Av Lima|Lima|Lima",
        DireccionLlegada: "Av Arequipa|Arequipa|Arequipa",
        HasManifest: false,
        VehiculoId: null,
        CarretaId: null,
        ChoferId: null,
        Igv: 0.18m,
        Year: 2024,
        Detalles: new List<DetalleGuiaInput>
        {
            new("Carga general", new Peso(10m), 100m, 100m, 1)
        });

    [Fact]
    public async Task Handle_Valid_RepoActualizarCalledWithUow()
    {
        // Arrange
        var command = BuildValidCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repo.Received(1).Actualizar(Arg.Any<Guia>(), _uow);
    }

    [Fact]
    public async Task Handle_EmptyDetalles_ThrowsValidationException()
    {
        // Arrange
        var command = BuildValidCommand() with { Detalles = new List<DetalleGuiaInput>() };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _repo.DidNotReceive().Actualizar(Arg.Any<Guia>(), Arg.Any<IUnitOfWork>());
    }

    [Fact]
    public async Task Handle_Valid_CommitCalled()
    {
        // Arrange
        var command = BuildValidCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _uow.Received(1).Commit();
    }
}
