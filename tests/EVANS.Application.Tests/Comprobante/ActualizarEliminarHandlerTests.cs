using EVANS.Application.Common;
using EVANS.Application.Comprobante.Commands;
using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Handlers;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.Comprobante;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Agg = EVANS.Domain.Comprobante.Comprobante;

namespace EVANS.Application.Tests.Comprobante;

public class ActualizarEliminarHandlerTests
{
    private readonly IComprobanteRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;

    public ActualizarEliminarHandlerTests()
    {
        _repo = Substitute.For<IComprobanteRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();
        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);
        // By default: ObtenerPorCodigo returns null (record not found — immutability check skipped)
        _repo.ObtenerPorCodigo(Arg.Any<int>()).Returns((ComprobanteDto?)null);
    }

    // --- Actualizar: repository.Actualizar called ---

    [Fact]
    public async Task Actualizar_ValidCommand_RepoActualizarCalled()
    {
        var handler = new ActualizarComprobanteCommandHandler(_repo, _uowFactory);
        var command = BuildActualizarCommand(codigo: 10);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repo.Received(1).Actualizar(Arg.Any<Agg>(), _uow);
        _uow.Received(1).Commit();
    }

    // --- NumeroComprobante NOT changed on Actualizar ---

    [Fact]
    public async Task Actualizar_AggregateNumeroNotChangedByHandler()
    {
        var handler = new ActualizarComprobanteCommandHandler(_repo, _uowFactory);

        Agg? captured = null;
        _repo.When(r => r.Actualizar(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>()))
             .Do(ci => captured = ci.Arg<Agg>());

        var command = BuildActualizarCommand(codigo: 10);
        await handler.Handle(command, CancellationToken.None);

        // Codigo is set from command
        captured.Should().NotBeNull();
        captured!.Codigo.Should().Be(10);
        // Numero matches what the command supplied (F001/000001 in the helper)
        captured.Numero.Serie.Should().Be("F001");
        captured.Numero.Numero.Should().Be("000001");
    }

    // --- W-1: NumeroComprobante immutability — different Numero throws DomainException ---

    [Fact]
    public async Task Actualizar_ConNumeroDistinto_LanzaDomainException()
    {
        // Arrange: repo returns existing comprobante with NumeroComprobante "F001-000001"
        _repo.ObtenerPorCodigo(42).Returns(BuildComprobanteDto(codigo: 42, numero: "F001-000001", tipo: 1));

        var handler = new ActualizarComprobanteCommandHandler(_repo, _uowFactory);

        // Command sends "F001"/"000002" — different Numero
        var command = new ActualizarComprobanteCommand(
            Codigo: 42,
            Serie: "F001",
            Numero: "000002",   // ← differs from DB "000001"
            Tipo: TipoComprobante.Factura,
            ClienteCodigo: 1,
            RucODni: "20123456789",
            Direccion: "Av Lima 123",
            Detalles: [new DetalleComprobanteInput(1, "Flete", 100m, 118m)],
            Year: 2024);

        // Act + Assert
        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            handler.Handle(command, CancellationToken.None));

        ex.Code.Should().Be("NUMERO_INMUTABLE");
        _repo.DidNotReceive().Actualizar(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>());
    }

    // --- W-1: Matching Numero passes the guard ---

    [Fact]
    public async Task Actualizar_ConNumeroIgual_Succeeds()
    {
        // Arrange: repo returns existing comprobante with NumeroComprobante "F001-000001"
        _repo.ObtenerPorCodigo(42).Returns(BuildComprobanteDto(codigo: 42, numero: "F001-000001", tipo: 1));

        var handler = new ActualizarComprobanteCommandHandler(_repo, _uowFactory);

        var command = new ActualizarComprobanteCommand(
            Codigo: 42,
            Serie: "F001",
            Numero: "000001",   // ← same as DB
            Tipo: TipoComprobante.Factura,
            ClienteCodigo: 1,
            RucODni: "20123456789",
            Direccion: "Av Lima 123",
            Detalles: [new DetalleComprobanteInput(1, "Flete", 100m, 118m)],
            Year: 2024);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repo.Received(1).Actualizar(Arg.Any<Agg>(), _uow);
    }

    // --- Eliminar: repository.Eliminar called ---

    [Fact]
    public async Task Eliminar_ValidCommand_RepoEliminarCalled()
    {
        var handler = new EliminarComprobanteCommandHandler(_repo, _uowFactory);
        var command = new EliminarComprobanteCommand(Codigo: 5, Year: 2024);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repo.Received(1).Eliminar(5, _uow);
        _uow.Received(1).Commit();
    }

    // --- Actualizar non-existent: currently returns success
    //     (DB will update 0 rows; infrastructure layer handles NOT_FOUND if needed)
    //     Handler-level: should still succeed (no exception from repo substitute)

    [Fact]
    public async Task Actualizar_NonExistentCodigo_RepoCalledSucceeds()
    {
        var handler = new ActualizarComprobanteCommandHandler(_repo, _uowFactory);
        var command = BuildActualizarCommand(codigo: 9999);

        // substitute does nothing by default (no throws), ObtenerPorCodigo returns null
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repo.Received(1).Actualizar(Arg.Any<Agg>(), _uow);
    }

    private static ActualizarComprobanteCommand BuildActualizarCommand(int codigo) =>
        new(
            Codigo: codigo,
            Serie: "F001",
            Numero: "000001",
            Tipo: TipoComprobante.Factura,
            ClienteCodigo: 1,
            RucODni: "20123456789",
            Direccion: "Av Lima 123",
            Detalles: new List<DetalleComprobanteInput>
            {
                new(1, "Flete Lima-Arequipa", 100m, 118m)
            },
            Year: 2024);

    private static ComprobanteDto BuildComprobanteDto(int codigo, string numero, int tipo) =>
        new(
            Codigo: codigo,
            NumeroComprobante: numero,
            Tipo: (TipoComprobante)tipo,
            Fecha: DateTime.Today,
            ClienteCodigo: 1,
            RucODni: "20123456789",
            Direccion: "Av Lima 123",
            Total: 118m,
            IGV: 18m,
            ValorVenta: 100m,
            Impreso: false,
            Detalles: []);
}
