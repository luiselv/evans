using EVANS.Application.Common;
using EVANS.Application.Identidad.Commands;
using EVANS.Application.Identidad.DTOs;
using EVANS.UI.WinForms.Identidad;
using MediatR;

namespace EVANS.UI.Tests.Identidad;

public sealed class FrmLoginTests
{
    [WinFormsFact]
    public async Task SubmitAsync_ValidCredentials_StoresAuthenticatedUser()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<AutenticarUsuarioCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<UsuarioSesionDto>.Ok(new UsuarioSesionDto("testuser", "Test User", true)));

        using var form = new frmLogin(mediator);
        form.SetTestCredentials("testuser", "testpass");

        var success = await form.SubmitAsync();

        success.Should().BeTrue();
        form.AuthenticatedUser.Should().NotBeNull();
        form.AuthenticatedUser!.NombreUsuario.Should().Be("testuser");
        await mediator.Received(1).Send(
            Arg.Is<AutenticarUsuarioCommand>(cmd =>
                cmd.NombreUsuario == "testuser" &&
                cmd.Clave == "testpass"),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task SubmitAsync_InvalidCredentials_ShowsErrorAndKeepsUserNull()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<AutenticarUsuarioCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<UsuarioSesionDto>.Fail("Invalid username or password."));

        using var form = new frmLogin(mediator);
        form.SetTestCredentials("testuser", "wrongpass");

        var success = await form.SubmitAsync();

        success.Should().BeFalse();
        form.AuthenticatedUser.Should().BeNull();
        form.ErrorMessage.Should().Be("Invalid username or password.");
    }
}
