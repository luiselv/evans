using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Recepcion.Commands;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using EVANS.Domain.Recepcion;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;
using Agg = EVANS.Domain.Recepcion.Recepcion;

namespace EVANS.Application.Tests.Recepcion;

public class ActualizarEliminarHandlerTests
{
    private readonly IRecepcionRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;

    private static readonly IReadOnlyList<DetalleRecepcionInput> OneDetalle =
        new[] { new DetalleRecepcionInput(1m, "Carga", 5m, "KG", 100m, "GR", "001") };

    private static Agg SampleAggregate()
    {
        var det = DetalleRecepcion.Crear(1m, "Carga", 5m, "KG", 100m, "GR", "001");
        var agg = Agg.Crear(DateTime.Today, 1, TipoDireccion.Agencia, "Dir",
            2, TipoDireccion.Agencia, "Dir2", 1, 1, null, null, 100m, null, 1, new[] { det });
        agg.SetCodigo(99);
        return agg;
    }

    public ActualizarEliminarHandlerTests()
    {
        _repo = Substitute.For<IRecepcionRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();
        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);
    }

    private ActualizarRecepcionCommand ValidActualizar(int codigo = 99) => new(
        Codigo: codigo,
        FechaEmision: DateTime.Today,
        RemitenteId: 1,
        TipoDirPartida: TipoDireccion.Agencia,
        DireccionPartida: "Dir",
        DestinatarioId: 2,
        TipoDirDestino: TipoDireccion.Agencia,
        DireccionDestino: "Dir2",
        DestinoId: 1,
        EstadoId: 1,
        Bultos: null,
        PesoTotal: null,
        CostoTotal: 100m,
        Observacion: null,
        Detalles: OneDetalle,
        Year: 2026);

    // A-03: not found → throws DomainException REC404, ActualizarAsync never called
    [Fact]
    public async Task Actualizar_NotFound_ThrowsREC404_ActualizarNeverCalled()
    {
        _repo.ObtenerPorCodigoAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<Agg?>(null));

        var handler = new ActualizarRecepcionCommandHandler(_repo, _uowFactory);

        var act = async () => await handler.Handle(ValidActualizar(), CancellationToken.None);

        await act.Should().ThrowAsync<Domain.Recepcion.DomainException>()
            .Where(e => e.Code == "REC404");
        await _repo.DidNotReceive().ActualizarAsync(Arg.Any<Agg>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>());
    }

    // A-04: happy path — ActualizarAsync called once
    [Fact]
    public async Task Actualizar_HappyPath_ActualizarAsyncCalledOnce()
    {
        _repo.ObtenerPorCodigoAsync(99, 2026, Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<Agg?>(SampleAggregate()));

        var handler = new ActualizarRecepcionCommandHandler(_repo, _uowFactory);

        var result = await handler.Handle(ValidActualizar(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _repo.Received(1).ActualizarAsync(Arg.Any<Agg>(), _uow, Arg.Any<CancellationToken>());
        _uow.Received(1).Commit();
    }

    // A-05: EliminarAsync called with correct codigo
    [Fact]
    public async Task Eliminar_HappyPath_EliminarAsyncCalledWithCorrectCodigo()
    {
        var handler = new EliminarRecepcionCommandHandler(_repo, _uowFactory);
        var cmd = new EliminarRecepcionCommand(Codigo: 77, Year: 2026);

        var result = await handler.Handle(cmd, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _repo.Received(1).EliminarAsync(77, _uow, Arg.Any<CancellationToken>());
        _uow.Received(1).Commit();
    }
}
