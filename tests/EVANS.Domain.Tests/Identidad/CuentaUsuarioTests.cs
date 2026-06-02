using EVANS.Domain.Identidad;
using FluentAssertions;

namespace EVANS.Domain.Tests.Identidad;

public sealed class CuentaUsuarioTests
{
    [Fact]
    public void Crear_ValidData_CreatesCuentaUsuario()
    {
        var usuario = CuentaUsuario.Crear(" jdoe ", "secret", " John Doe ", true, 1);

        usuario.NombreUsuario.Should().Be("jdoe");
        usuario.Clave.Should().Be("secret");
        usuario.NombreCompleto.Should().Be("John Doe");
        usuario.EsAdministrador.Should().BeTrue();
        usuario.EstadoCodigo.Should().Be(1);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Crear_MissingUsername_ThrowsDomainException(string username)
    {
        Action act = () => CuentaUsuario.Crear(username, "secret", "John Doe", false, 1);

        act.Should().Throw<DomainException>()
            .WithMessage("Username is required.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Crear_MissingPassword_ThrowsDomainException(string password)
    {
        Action act = () => CuentaUsuario.Crear("jdoe", password, "John Doe", false, 1);

        act.Should().Throw<DomainException>()
            .WithMessage("Password is required.");
    }
}
