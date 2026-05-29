using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using EVANS.Application.Recepcion.Queries;
using EVANS.Domain.Recepcion;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Agg = EVANS.Domain.Recepcion.Recepcion;
using Det = EVANS.Domain.Recepcion.DetalleRecepcion;

namespace EVANS.Application.Tests.Recepcion;

public class QueryHandlerTests
{
    private readonly IRecepcionRepository _repo;

    public QueryHandlerTests()
    {
        _repo = Substitute.For<IRecepcionRepository>();
    }

    private static Agg BuildAggregate(int codigo)
    {
        var det = Det.Crear(2m, "Carga", 5m, "KG", 50m, "GR", "001");
        var det2 = Det.Crear(1m, "Otro", 2m, "UND", 30m, "GR", "002");
        var agg = Agg.Crear(DateTime.Today, 1, TipoDireccion.Agencia, "Dir",
            2, TipoDireccion.Agencia, "Dir2", 1, 1, null, null, 80m, null, 1, new[] { det, det2 });
        agg.SetCodigo(codigo);
        return agg;
    }

    // A-06: ObtenerPorCodigo — returns null when not found
    [Fact]
    public async Task Obtener_NotFound_ReturnsNullResult()
    {
        _repo.ObtenerPorCodigoAsync(999, 2026, Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<Agg?>(null));

        var handler = new ObtenerRecepcionPorCodigoQueryHandler(_repo);
        var result = await handler.Handle(
            new ObtenerRecepcionPorCodigoQuery(999, 2026), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    // A-07: ObtenerPorCodigo — returns DTO with 2 detalles when found
    [Fact]
    public async Task Obtener_Found_ReturnsDtoWithDetalles()
    {
        _repo.ObtenerPorCodigoAsync(1, 2026, Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<Agg?>(BuildAggregate(1)));

        var handler = new ObtenerRecepcionPorCodigoQueryHandler(_repo);
        var result = await handler.Handle(
            new ObtenerRecepcionPorCodigoQuery(1, 2026), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Codigo.Should().Be(1);
        result.Value.Detalles.Should().HaveCount(2);
    }

    // A-08: BuscarRecepciones — repository called with DateRange.Hoy
    [Fact]
    public async Task Buscar_Hoy_RepositoryCalledWithCorrectRange()
    {
        var rango = DateRange.Hoy();
        _repo.BuscarPorRangoFechasAsync(Arg.Any<DateRange>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<IReadOnlyList<RecepcionListItemDto>>(Array.Empty<RecepcionListItemDto>()));

        var handler = new BuscarRecepcionesQueryHandler(_repo);
        var query = new BuscarRecepcionesQuery(rango, 2026);

        await handler.Handle(query, CancellationToken.None);

        await _repo.Received(1).BuscarPorRangoFechasAsync(
            Arg.Is<DateRange>(r => r.Inicio == rango.Inicio && r.Fin == rango.Fin),
            2026,
            Arg.Any<CancellationToken>());
    }

    // A-09: BuscarRecepciones — returns empty list when repo returns empty
    [Fact]
    public async Task Buscar_EmptyResult_ReturnsEmptyList()
    {
        _repo.BuscarPorRangoFechasAsync(Arg.Any<DateRange>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<IReadOnlyList<RecepcionListItemDto>>(Array.Empty<RecepcionListItemDto>()));

        var handler = new BuscarRecepcionesQueryHandler(_repo);
        var result = await handler.Handle(
            new BuscarRecepcionesQuery(DateRange.Hoy(), 2026), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
