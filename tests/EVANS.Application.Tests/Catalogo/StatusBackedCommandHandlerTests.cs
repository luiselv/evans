using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using NSubstitute;

namespace EVANS.Application.Tests.Catalogo;

public sealed class StatusBackedCommandHandlerTests
{
    [Fact]
    public async Task CreateVehiculoCommandHandler_ValidCommand_AddsVehiculo()
    {
        var repo = Substitute.For<IRepository<Vehiculo>>();
        repo.AddAsync(Arg.Any<Vehiculo>(), Arg.Any<CancellationToken>()).Returns(10);

        var result = await new CreateVehiculoCommandHandler(repo).Handle(
            new CreateVehiculoCommand("Toyota", "abc-123", "C2", "CERT", 1),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(10);
        await repo.Received(1).AddAsync(
            Arg.Is<Vehiculo>(v => v.Placa == "ABC-123" && v.EstadoCodigo == CatalogoEstado.Activo),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateVehiculoCommandHandler_ExistingEntity_UpdatesVehiculo()
    {
        var repo = Substitute.For<IRepository<Vehiculo>>();
        repo.GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(Vehiculo.Materializar(10, "Old", "OLD-123", "C2", null, 1, CatalogoEstado.Activo));

        var result = await new UpdateVehiculoCommandHandler(repo).Handle(
            new UpdateVehiculoCommand(10, "Toyota", "abc-123", "C3", "CERT", 1),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await repo.Received(1).UpdateAsync(
            Arg.Is<Vehiculo>(v => v.Codigo == 10 && v.Placa == "ABC-123" && v.ConfiguracionVehicular == "C3"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeactivateVehiculoCommandHandler_MissingEntity_ReturnsFailure()
    {
        var repo = Substitute.For<IRepository<Vehiculo>>();
        repo.GetByIdAsync(10, Arg.Any<CancellationToken>()).Returns((Vehiculo?)null);

        var result = await new DeactivateVehiculoCommandHandler(repo)
            .Handle(new DeactivateVehiculoCommand(10), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await repo.DidNotReceive().DeactivateAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateCarretaCommandHandler_ValidCommand_AddsCarreta()
    {
        var repo = Substitute.For<IRepository<Carreta>>();
        repo.AddAsync(Arg.Any<Carreta>(), Arg.Any<CancellationToken>()).Returns(11);

        var result = await new CreateCarretaCommandHandler(repo).Handle(
            new CreateCarretaCommand("xyz-789", "Volvo", "CERT", 1, CatalogoEstado.Inactivo),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(11);
        await repo.Received(1).AddAsync(
            Arg.Is<Carreta>(c => c.Placa == "XYZ-789" && c.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateCarretaCommandHandler_ExistingEntity_UpdatesSelectedStatus()
    {
        var repo = Substitute.For<IRepository<Carreta>>();
        repo.GetByIdAsync(11, Arg.Any<CancellationToken>())
            .Returns(Carreta.Materializar(11, "XYZ-789", "Volvo", "CERT", 1, CatalogoEstado.Activo));

        var result = await new UpdateCarretaCommandHandler(repo).Handle(
            new UpdateCarretaCommand(11, "abc-123", "Scania", "NEW", 2, CatalogoEstado.Inactivo),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await repo.Received(1).UpdateAsync(
            Arg.Is<Carreta>(c =>
                c.Codigo == 11
                && c.Placa == "ABC-123"
                && c.Marca == "Scania"
                && c.EmpresaCodigo == 2
                && c.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateChoferCommandHandler_ValidCommand_AddsChofer()
    {
        var repo = Substitute.For<IRepository<Chofer>>();
        repo.AddAsync(Arg.Any<Chofer>(), Arg.Any<CancellationToken>()).Returns(12);

        var result = await new CreateChoferCommandHandler(repo).Handle(
            new CreateChoferCommand("Juan Perez", "A123", "555", "Av Lima", 1, CatalogoEstado.Inactivo),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(12);
        await repo.Received(1).AddAsync(
            Arg.Is<Chofer>(c => c.NombreCompleto == "Juan Perez" && c.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateChoferCommandHandler_ExistingEntity_UpdatesSelectedStatus()
    {
        var repo = Substitute.For<IRepository<Chofer>>();
        repo.GetByIdAsync(12, Arg.Any<CancellationToken>())
            .Returns(Chofer.Materializar(12, "Juan Perez", "A123", "555", "Av Lima", 1, CatalogoEstado.Activo));

        var result = await new UpdateChoferCommandHandler(repo).Handle(
            new UpdateChoferCommand(12, "Juan Perez", "B456", "777", "Av Norte", 2, CatalogoEstado.Inactivo),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await repo.Received(1).UpdateAsync(
            Arg.Is<Chofer>(c =>
                c.Codigo == 12
                && c.Licencia == "B456"
                && c.EmpresaCodigo == 2
                && c.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateDestinoCommandHandler_ValidCommand_AddsDestino()
    {
        var repo = Substitute.For<IRepository<Destino>>();
        repo.AddAsync(Arg.Any<Destino>(), Arg.Any<CancellationToken>()).Returns(13);

        var result = await new CreateDestinoCommandHandler(repo).Handle(
            new CreateDestinoCommand("Lima", 10, CatalogoEstado.Inactivo),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(13);
        await repo.Received(1).AddAsync(
            Arg.Is<Destino>(d => d.Descripcion == "Lima" && d.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateDestinoCommandHandler_ExistingEntity_UpdatesSelectedStatus()
    {
        var repo = Substitute.For<IRepository<Destino>>();
        repo.GetByIdAsync(13, Arg.Any<CancellationToken>())
            .Returns(Destino.Materializar(13, "Lima", 10, CatalogoEstado.Activo));

        var result = await new UpdateDestinoCommandHandler(repo).Handle(
            new UpdateDestinoCommand(13, "Callao", 15, CatalogoEstado.Inactivo),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await repo.Received(1).UpdateAsync(
            Arg.Is<Destino>(d =>
                d.Codigo == 13
                && d.Descripcion == "Callao"
                && d.DistanciaVirtual == 15
                && d.EstadoCodigo == CatalogoEstado.Inactivo),
            Arg.Any<CancellationToken>());
    }
}
