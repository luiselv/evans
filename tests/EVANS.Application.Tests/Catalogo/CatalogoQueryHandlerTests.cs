using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Ports;
using EVANS.Application.Catalogo.Queries;
using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;
using NSubstitute;

namespace EVANS.Application.Tests.Catalogo;

public class CatalogoQueryHandlerTests
{
    [Fact]
    public async Task ListEmpresasQuery_ReturnsDtosFromRepository()
    {
        Ruc.TryCreate("20123456789", out var ruc).Should().BeTrue();
        var repo = Substitute.For<IRepository<Empresa>>();
        repo.ListActiveAsync(Arg.Any<CancellationToken>())
            .Returns([Empresa.Materializar(7, "TRANSPORT SA", "Av Lima", "123", ruc, true, CatalogoEstado.Activo)]);

        var result = await new ListEmpresasQueryHandler(repo).Handle(new ListEmpresasQuery(), CancellationToken.None);

        result.Should().ContainSingle().Which.Should().BeEquivalentTo(
            new EmpresaDto(7, "TRANSPORT SA", "Av Lima", "123", "20123456789", true, CatalogoEstado.Activo));
        await repo.Received(1).ListActiveAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetClienteByIdQuery_ReturnsClienteWithDirecciones()
    {
        var repo = Substitute.For<IClienteRepository>();
        repo.GetByIdAsync(3, Arg.Any<CancellationToken>())
            .Returns(Cliente.Materializar(3, "ACME", 1, "20123456789", "555", "ops@acme.test",
                [new Direccion("Av Lima", "Lima", "Lima")]));

        var result = await new GetClienteByIdQueryHandler(repo).Handle(new GetClienteByIdQuery(3), CancellationToken.None);

        result.Should().BeEquivalentTo(new ClienteDto(
            3,
            "ACME",
            1,
            "20123456789",
            "555",
            "ops@acme.test",
            [new DireccionDto("Av Lima", "Lima", "Lima")]));
    }

    [Fact]
    public async Task ListEstadosQuery_UsesNonStatusBackedRepository()
    {
        var repo = Substitute.For<IEstadoRepository>();
        repo.ListAsync(Arg.Any<CancellationToken>())
            .Returns([Estado.Materializar(1, "Activo")]);

        var result = await new ListEstadosQueryHandler(repo).Handle(new ListEstadosQuery(), CancellationToken.None);

        result.Should().ContainSingle().Which.Should().Be(new EstadoDto(1, "Activo"));
        await repo.Received(1).ListAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetTipoIdentificacionByIdQuery_ReturnsComputedLongitud()
    {
        var repo = Substitute.For<ITipoIdentificacionRepository>();
        repo.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(TipoIdentificacion.Materializar(2, "DNI"));

        var result = await new GetTipoIdentificacionByIdQueryHandler(repo)
            .Handle(new GetTipoIdentificacionByIdQuery(2), CancellationToken.None);

        result.Should().Be(new TipoIdentificacionDto(2, "DNI", 8));
    }

    [Fact]
    public async Task ListAgenciasQuery_MapsReadOnlySchemaShape()
    {
        var repo = Substitute.For<IAgenciaRepository>();
        repo.ListActiveAsync(Arg.Any<CancellationToken>())
            .Returns([Agencia.Materializar(4, "Av Lima", 10, CatalogoEstado.Activo)]);

        var result = await new ListAgenciasQueryHandler(repo).Handle(new ListAgenciasQuery(), CancellationToken.None);

        result.Should().ContainSingle().Which.Should().Be(
            new AgenciaDto(4, "Av Lima", 10, CatalogoEstado.Activo));
    }
}
