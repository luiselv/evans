using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class ClienteTests
{
    [Fact]
    public void Crear_ValidRucCliente_PreservesNroIdentificacionAsString()
    {
        var cliente = Cliente.Crear("ACME", 1, "20123456789", 11, null, null, null, null, OneDireccion());

        cliente.RazonSocial.Should().Be("ACME");
        cliente.NroIdentificacion.Should().Be("20123456789");
        cliente.Direcciones.Should().ContainSingle();
    }

    [Fact]
    public void Crear_ValidDniWithLeadingZero_PreservesLeadingZero()
    {
        var cliente = Cliente.Crear("ACME", 2, "07654321", 8, null, null, null, null, OneDireccion());

        cliente.NroIdentificacion.Should().Be("07654321");
    }

    [Fact]
    public void Crear_LengthMismatch_ThrowsDomainException()
    {
        Action act = () => Cliente.Crear("ACME", 2, "123456789", 8, null, null, null, null, OneDireccion());

        act.Should().Throw<DomainException>().Where(e => e.Code == "CAT-CLI-002");
    }

    [Fact]
    public void Crear_WithoutDireccion_AllowsLegacyCliente()
    {
        var cliente = Cliente.Crear("ACME", 1, "20123456789", 11, null, null, null, null, []);

        cliente.Direcciones.Should().BeEmpty();
    }

    [Fact]
    public void Crear_PreservesFaxAndRepresentante()
    {
        var cliente = Cliente.Crear("ACME", 3, "PASS12345", 9, "555", "123", "ops@acme.test", "ANA", []);

        cliente.Fax.Should().Be("123");
        cliente.Representante.Should().Be("ANA");
        cliente.NroIdentificacion.Should().Be("PASS12345");
    }

    private static IReadOnlyList<Direccion> OneDireccion() => [new("Av Lima", "Lima", "Lima")];
}
