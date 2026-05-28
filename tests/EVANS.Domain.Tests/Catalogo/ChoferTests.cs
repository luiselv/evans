using EVANS.Domain.Catalogo;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class ChoferTests
{
    [Fact]
    public void Crear_ValidData_SetsActiveState()
    {
        var chofer = Chofer.Crear("JUAN PEREZ", "Q12345678", "999", "Av Lima", 10);

        chofer.NombreCompleto.Should().Be("JUAN PEREZ");
        chofer.EstadoCodigo.Should().Be(CatalogoEstado.Activo);
    }

    [Fact]
    public void Crear_WithoutLicencia_ThrowsDomainException()
    {
        Action act = () => Chofer.Crear("JUAN PEREZ", " ", null, null, 10);

        act.Should().Throw<DomainException>().Where(e => e.Code == "CAT-CHF-002");
    }
}
