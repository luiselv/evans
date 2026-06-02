using EVANS.Application.Identidad.Commands;
using EVANS.Application.Identidad.Ports;
using EVANS.Domain.Identidad;
using NSubstitute;

namespace EVANS.Application.Tests.Identidad;

public sealed class UsuarioCommandHandlerTests
{
    [Fact]
    public async Task CrearUsuarioCommandHandler_ValidCommand_AddsUsuario()
    {
        var repo = Substitute.For<IUsuarioRepository>();
        repo.AddAsync(Arg.Any<CuentaUsuario>(), Arg.Any<CancellationToken>()).Returns(10);

        var result = await new CrearUsuarioCommandHandler(repo).Handle(
            new CrearUsuarioCommand("jdoe", "secret", "John Doe", true, 1),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(10);
        await repo.Received(1).AddAsync(
            Arg.Is<CuentaUsuario>(usuario =>
                usuario.NombreUsuario == "jdoe" &&
                usuario.NombreCompleto == "John Doe" &&
                usuario.EsAdministrador),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CrearUsuarioCommandHandler_InvalidCommand_ReturnsFailureWithoutRepositoryCall()
    {
        var repo = Substitute.For<IUsuarioRepository>();

        var result = await new CrearUsuarioCommandHandler(repo).Handle(
            new CrearUsuarioCommand("", "secret", "John Doe", false, 1),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await repo.DidNotReceive().AddAsync(Arg.Any<CuentaUsuario>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ActualizarUsuarioCommandHandler_MissingUsuario_ReturnsFailure()
    {
        var repo = Substitute.For<IUsuarioRepository>();
        repo.GetByIdAsync(10, Arg.Any<CancellationToken>()).Returns((CuentaUsuario?)null);

        var result = await new ActualizarUsuarioCommandHandler(repo).Handle(
            new ActualizarUsuarioCommand(10, "jdoe", "secret", "John Doe", false, 1),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await repo.DidNotReceive().UpdateAsync(Arg.Any<CuentaUsuario>(), Arg.Any<CancellationToken>());
    }
}
