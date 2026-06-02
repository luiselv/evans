using EVANS.Application.Identidad.Commands;
using EVANS.Application.Identidad.Ports;
using EVANS.Domain.Identidad;
using NSubstitute;

namespace EVANS.Application.Tests.Identidad;

public sealed class AutenticarUsuarioCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCredentials_ReturnsAuthenticatedUser()
    {
        var repo = Substitute.For<IUsuarioRepository>();
        repo.AuthenticateAsync("testuser", "testpass", Arg.Any<CancellationToken>())
            .Returns(Usuario.Autenticado("testuser", "Test User", true));

        var result = await new AutenticarUsuarioCommandHandler(repo)
            .Handle(new AutenticarUsuarioCommand("testuser", "testpass"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.NombreUsuario.Should().Be("testuser");
        result.Value.EsAdministrador.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_InvalidCredentials_ReturnsFailure()
    {
        var repo = Substitute.For<IUsuarioRepository>();
        repo.AuthenticateAsync("testuser", "wrongpass", Arg.Any<CancellationToken>())
            .Returns((Usuario?)null);

        var result = await new AutenticarUsuarioCommandHandler(repo)
            .Handle(new AutenticarUsuarioCommand("testuser", "wrongpass"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid username or password.");
    }

    [Theory]
    [InlineData("", "testpass")]
    [InlineData("testuser", "")]
    [InlineData("   ", "testpass")]
    public async Task Handle_MissingCredentials_ReturnsFailureWithoutRepositoryCall(string username, string password)
    {
        var repo = Substitute.For<IUsuarioRepository>();

        var result = await new AutenticarUsuarioCommandHandler(repo)
            .Handle(new AutenticarUsuarioCommand(username, password), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await repo.DidNotReceive().AuthenticateAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }
}
