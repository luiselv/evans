using EVANS.Domain.Catalogo;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class EstadoTests
{
    [Fact]
    public void Crear_ValidDescripcion_SetsValue()
    {
        var estado = Estado.Crear("Activo");

        estado.Descripcion.Should().Be("Activo");
    }

    [Fact]
    public void Crear_BlankDescripcion_ThrowsDomainException()
    {
        Action act = () => Estado.Crear(" ");

        act.Should().Throw<DomainException>().Where(e => e.Code == "CAT-EST-001");
    }
}
