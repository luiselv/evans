using EVANS.Application.Common;
using EVANS.Application.Comprobante.Commands;
using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Handlers;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.Comprobante.Queries;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.Comprobante;
using FluentAssertions;
using NSubstitute;
using Agg = EVANS.Domain.Comprobante.Comprobante;

namespace EVANS.Application.Tests.Comprobante;

public class MarcarImpresoHandlerTests
{
    private readonly IComprobanteRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;

    public MarcarImpresoHandlerTests()
    {
        _repo = Substitute.For<IComprobanteRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();
        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);
    }

    // --- MarcarImpreso: repository.MarcarImpreso called with correct codigo ---

    [Fact]
    public async Task MarcarImpreso_ValidCommand_RepoCalledWithCorrectCodigo()
    {
        var handler = new MarcarImpresoCommandHandler(_repo, _uowFactory);
        var command = new MarcarImpresoCommand(Codigo: 42, Year: 2024);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repo.Received(1).MarcarImpreso(42, _uow);
        _uow.Received(1).Commit();
    }

    // --- Buscar: filter mapped correctly; empty result not error ---

    [Fact]
    public async Task Buscar_ValidFilter_RepoCalledAndReturnsResult()
    {
        var fakeResults = new List<ComprobanteResumenDto>
        {
            new(1, "F001-000001", TipoComprobante.Factura, DateTime.Today, 5, 118m, false)
        };
        _repo.Buscar(Arg.Any<BuscarComprobantesFiltro>()).Returns(fakeResults);

        var handler = new BuscarComprobantesQueryHandler(_repo);
        var query = new BuscarComprobantesQuery(
            Desde: DateTime.Today.AddDays(-30),
            Hasta: DateTime.Today,
            ClienteCodigo: 5,
            Tipo: TipoComprobante.Factura,
            SoloImpreso: false,
            Year: 2024);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(1);
        _repo.Received(1).Buscar(Arg.Is<BuscarComprobantesFiltro>(f =>
            f.ClienteCodigo == 5 &&
            f.Tipo == TipoComprobante.Factura));
    }

    [Fact]
    public async Task Buscar_EmptyResult_ReturnsEmptyListNoError()
    {
        _repo.Buscar(Arg.Any<BuscarComprobantesFiltro>())
             .Returns(new List<ComprobanteResumenDto>());

        var handler = new BuscarComprobantesQueryHandler(_repo);
        var query = new BuscarComprobantesQuery(null, null, null, null, null, 2024);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }

    // --- ObtenerPorCodigo: not found returns null ---

    [Fact]
    public async Task ObtenerPorCodigo_NotFound_ReturnsNull()
    {
        _repo.ObtenerPorCodigo(Arg.Any<int>()).Returns((ComprobanteDto?)null);

        var handler = new ObtenerComprobantePorCodigoQueryHandler(_repo);
        var query = new ObtenerComprobantePorCodigoQuery(9999);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task ObtenerPorCodigo_Found_ReturnsDto()
    {
        var fakeDto = new ComprobanteDto(
            1, "F001-000001", TipoComprobante.Factura,
            DateTime.Today, 5, "20123456789", "Av Lima", 118m, 18m, 100m, false,
            new List<DetalleComprobanteDto>());
        _repo.ObtenerPorCodigo(1).Returns(fakeDto);

        var handler = new ObtenerComprobantePorCodigoQueryHandler(_repo);
        var query = new ObtenerComprobantePorCodigoQuery(1);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Codigo.Should().Be(1);
    }
}
