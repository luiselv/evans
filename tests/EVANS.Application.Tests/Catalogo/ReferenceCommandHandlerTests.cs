using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;
using NSubstitute;

namespace EVANS.Application.Tests.Catalogo;

public sealed class ReferenceCommandHandlerTests
{
    [Fact]
    public async Task CreateClienteCommandHandler_ValidCommand_AddsCliente()
    {
        var repo = Substitute.For<IClienteRepository>();
        repo.AddAsync(Arg.Any<Cliente>(), Arg.Any<CancellationToken>()).Returns(20);

        var result = await new CreateClienteCommandHandler(repo).Handle(
            new CreateClienteCommand(
                "ACME",
                1,
                "20123456789",
                11,
                "555",
                "ops@acme.test",
                [new DireccionDto("Av Lima", "Lima", "Lima")]),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(20);
        await repo.Received(1).AddAsync(
            Arg.Is<Cliente>(c => c.RazonSocial == "ACME" && c.Direcciones.Count == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateClienteCommandHandler_MissingCliente_ReturnsFailure()
    {
        var repo = Substitute.For<IClienteRepository>();
        repo.GetByIdAsync(20, Arg.Any<CancellationToken>()).Returns((Cliente?)null);

        var result = await new UpdateClienteCommandHandler(repo).Handle(
            new UpdateClienteCommand(
                20,
                "ACME",
                1,
                "20123456789",
                11,
                null,
                null,
                [new DireccionDto("Av Lima", "Lima", "Lima")]),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await repo.DidNotReceive().UpdateAsync(Arg.Any<Cliente>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateEstadoCommandHandler_ValidCommand_AddsEstado()
    {
        var repo = Substitute.For<IEstadoRepository>();
        repo.AddAsync(Arg.Any<Estado>(), Arg.Any<CancellationToken>()).Returns(2);

        var result = await new CreateEstadoCommandHandler(repo)
            .Handle(new CreateEstadoCommand("Inactivo"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(2);
        await repo.Received(1).AddAsync(
            Arg.Is<Estado>(estado => estado.Descripcion == "Inactivo"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateTipoIdentificacionCommandHandler_ExistingType_UpdatesTipo()
    {
        var repo = Substitute.For<ITipoIdentificacionRepository>();
        repo.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(TipoIdentificacion.Materializar(2, "DNI"));

        var result = await new UpdateTipoIdentificacionCommandHandler(repo)
            .Handle(new UpdateTipoIdentificacionCommand(2, "Documento Nacional"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await repo.Received(1).UpdateAsync(
            Arg.Is<TipoIdentificacion>(tipo => tipo.Codigo == 2 && tipo.Descripcion == "Documento Nacional"),
            Arg.Any<CancellationToken>());
    }
}
