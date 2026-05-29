using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Handlers;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.Manifiesto.Queries;
using FluentAssertions;
using NSubstitute;

namespace EVANS.Application.Tests.Manifiesto;

public class QueryHandlerTests
{
    private readonly IManifiestoRepository _repo;

    public QueryHandlerTests()
    {
        _repo = Substitute.For<IManifiestoRepository>();
    }

    private static ManifiestoDto BuildManifiestoDto(int codigo) =>
        new(Codigo: codigo, Numero: "2024-1", Fecha: DateTime.Today,
            TransportistaCodigo: 5, TransportistaNombre: "Transportes SA",
            VehiculoCodigo: 3, VehiculoPlaca: "ABC-123",
            CarretaCodigo: null, CarretaPlaca: null,
            ChoferCodigo: 7, ChoferNombre: "Juan Perez",
            Importe: 100m, Peso: 50m, NroGuias: 2,
            EstadoCodigo: 1, EstadoNombre: "Activo",
            UsuarioCodigo: 2,
            Lineas: new List<ManifiestoLineaDto>());

    // A-10: ObtenerPorCodigo retorna DTO
    [Fact]
    public async Task ObtenerPorCodigo_Found_RetornaDto()
    {
        _repo.ObtenerPorCodigoAsync(42, 2024, Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<ManifiestoDto?>(BuildManifiestoDto(42)));

        var handler = new ObtenerManifiestoPorCodigoQueryHandler(_repo);
        var result = await handler.Handle(new ObtenerManifiestoPorCodigoQuery(42, 2024), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Codigo.Should().Be(42);
    }

    // A-10 not-found variant
    [Fact]
    public async Task ObtenerPorCodigo_NotFound_RetornaNull()
    {
        _repo.ObtenerPorCodigoAsync(999, 2024, Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<ManifiestoDto?>(null));

        var handler = new ObtenerManifiestoPorCodigoQueryHandler(_repo);
        var result = await handler.Handle(new ObtenerManifiestoPorCodigoQuery(999, 2024), CancellationToken.None);

        result.Should().BeNull();
    }

    // A-11: Buscar retorna lista
    [Fact]
    public async Task Buscar_RetornaLista()
    {
        var filtro = new BuscarManifiestosFiltro(Year: 2024);
        var expected = new List<ManifiestoResumenDto>
        {
            new(1, "2024-1", DateTime.Today, "Transportes SA", "ABC-123", "Juan Perez", 100m, 2, "Activo"),
            new(2, "2024-2", DateTime.Today, "Transportes XYZ", "DEF-456", "Pedro Lopez", 200m, 3, "Cerrado"),
        };

        _repo.BuscarAsync(filtro, 2024, Arg.Any<CancellationToken>())
             .Returns(Task.FromResult<IReadOnlyList<ManifiestoResumenDto>>(expected));

        var handler = new BuscarManifiestosQueryHandler(_repo);
        var result = await handler.Handle(new BuscarManifiestosQuery(filtro), CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Numero.Should().Be("2024-1");
    }
}
