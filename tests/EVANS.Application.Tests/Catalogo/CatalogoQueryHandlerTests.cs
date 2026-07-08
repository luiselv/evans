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
    public async Task ListEmpresasMaintenanceQuery_ReturnsAllStatuses()
    {
        Ruc.TryCreate("20123456789", out var activeRuc).Should().BeTrue();
        Ruc.TryCreate("20987654321", out var inactiveRuc).Should().BeTrue();
        var repo = Substitute.For<IEmpresaMaintenanceRepository>();
        repo.ListAllAsync(Arg.Any<CancellationToken>())
            .Returns(
            [
                Empresa.Materializar(1, "EVANS CARGO", "Av Lima", "123", activeRuc, true, CatalogoEstado.Activo),
                Empresa.Materializar(2, "OLD CARRIER", "Av Norte", "555", inactiveRuc, false, CatalogoEstado.Inactivo)
            ]);

        var result = await new ListEmpresasMaintenanceQueryHandler(repo)
            .Handle(new ListEmpresasMaintenanceQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(
        [
            new EmpresaDto(1, "EVANS CARGO", "Av Lima", "123", "20123456789", true, CatalogoEstado.Activo),
            new EmpresaDto(2, "OLD CARRIER", "Av Norte", "555", "20987654321", false, CatalogoEstado.Inactivo)
        ]);
        await repo.Received(1).ListAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetClienteByIdQuery_ReturnsClienteWithDirecciones()
    {
        var repo = Substitute.For<IClienteRepository>();
        repo.GetByIdAsync(3, Arg.Any<CancellationToken>())
            .Returns(Cliente.Materializar(3, "ACME", 1, "20123456789", "555", "123", "ops@acme.test", "ANA",
                [new Direccion("Av Lima", "Lima", "Lima")]));

        var result = await new GetClienteByIdQueryHandler(repo).Handle(new GetClienteByIdQuery(3), CancellationToken.None);

        result.Should().BeEquivalentTo(new ClienteDto(
            3,
            "ACME",
            1,
            "20123456789",
            "555",
            "123",
            "ops@acme.test",
            "ANA",
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
        repo.ListAsync(Arg.Any<CancellationToken>())
            .Returns([Agencia.Materializar(4, "Av Lima", 10, CatalogoEstado.Inactivo)]);

        var result = await new ListAgenciasQueryHandler(repo).Handle(new ListAgenciasQuery(), CancellationToken.None);

        result.Should().ContainSingle().Which.Should().Be(
            new AgenciaDto(4, "Av Lima", 10, CatalogoEstado.Inactivo));
        await repo.Received(1).ListAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ListChoferesMaintenanceQuery_ReturnsAllStatuses()
    {
        var repo = Substitute.For<IChoferMaintenanceRepository>();
        repo.ListAllAsync(Arg.Any<CancellationToken>())
            .Returns(
            [
                Chofer.Materializar(1, "Juan Perez", "Q12345678", "555", "Av Lima", 1, CatalogoEstado.Activo),
                Chofer.Materializar(2, "Pedro Diaz", "Q87654321", null, null, 1, CatalogoEstado.Inactivo)
            ]);

        var result = await new ListChoferesMaintenanceQueryHandler(repo)
            .Handle(new ListChoferesMaintenanceQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(
        [
            new ChoferDto(1, "Juan Perez", "Q12345678", "555", "Av Lima", 1, CatalogoEstado.Activo),
            new ChoferDto(2, "Pedro Diaz", "Q87654321", null, null, 1, CatalogoEstado.Inactivo)
        ]);
        await repo.Received(1).ListAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ListVehiculosMaintenanceQuery_ReturnsAllStatuses()
    {
        var repo = Substitute.For<IVehiculoMaintenanceRepository>();
        repo.ListAllAsync(Arg.Any<CancellationToken>())
            .Returns(
            [
                Vehiculo.Materializar(1, "Volvo", "ABC-123", "C2", "CERT", 1, CatalogoEstado.Activo),
                Vehiculo.Materializar(2, "Scania", "XYZ-789", "C3", null, 1, CatalogoEstado.Inactivo)
            ]);

        var result = await new ListVehiculosMaintenanceQueryHandler(repo)
            .Handle(new ListVehiculosMaintenanceQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(
        [
            new VehiculoDto(1, "Volvo", "ABC-123", "C2", "CERT", 1, CatalogoEstado.Activo),
            new VehiculoDto(2, "Scania", "XYZ-789", "C3", null, 1, CatalogoEstado.Inactivo)
        ]);
        await repo.Received(1).ListAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ListCarretasMaintenanceQuery_ReturnsAllStatuses()
    {
        var repo = Substitute.For<ICarretaMaintenanceRepository>();
        repo.ListAllAsync(Arg.Any<CancellationToken>())
            .Returns(
            [
                Carreta.Materializar(1, "XYZ-789", "Volvo", "CERT", 1, CatalogoEstado.Activo),
                Carreta.Materializar(2, "ABC-123", "Scania", null, 1, CatalogoEstado.Inactivo)
            ]);

        var result = await new ListCarretasMaintenanceQueryHandler(repo)
            .Handle(new ListCarretasMaintenanceQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(
        [
            new CarretaDto(1, "XYZ-789", "Volvo", "CERT", 1, CatalogoEstado.Activo),
            new CarretaDto(2, "ABC-123", "Scania", null, 1, CatalogoEstado.Inactivo)
        ]);
        await repo.Received(1).ListAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ListDestinosMaintenanceQuery_ReturnsAllStatuses()
    {
        var repo = Substitute.For<IDestinoMaintenanceRepository>();
        repo.ListAllAsync(Arg.Any<CancellationToken>())
            .Returns(
            [
                Destino.Materializar(1, "Lima", 0, CatalogoEstado.Activo),
                Destino.Materializar(2, "Callao", 15, CatalogoEstado.Inactivo)
            ]);

        var result = await new ListDestinosMaintenanceQueryHandler(repo)
            .Handle(new ListDestinosMaintenanceQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(
        [
            new DestinoDto(1, "Lima", 0, CatalogoEstado.Activo),
            new DestinoDto(2, "Callao", 15, CatalogoEstado.Inactivo)
        ]);
        await repo.Received(1).ListAllAsync(Arg.Any<CancellationToken>());
    }
}
