using EVANS.Domain.Identidad;
using FluentAssertions;

namespace EVANS.Domain.Tests.Identidad;

public sealed class UsuarioTests
{
    [Fact]
    public void Autenticado_ValidData_CreatesUsuario()
    {
        var usuario = Usuario.Autenticado(" testuser ", " Test User ", true);

        usuario.NombreUsuario.Should().Be("testuser");
        usuario.NombreCompleto.Should().Be("Test User");
        usuario.EsAdministrador.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Autenticado_MissingUsername_ThrowsDomainException(string username)
    {
        Action act = () => Usuario.Autenticado(username, "Test User", false);

        act.Should().Throw<DomainException>()
            .WithMessage("Username is required.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Autenticado_MissingFullName_ThrowsDomainException(string fullName)
    {
        Action act = () => Usuario.Autenticado("testuser", fullName, false);

        act.Should().Throw<DomainException>()
            .WithMessage("Full name is required.");
    }
}
