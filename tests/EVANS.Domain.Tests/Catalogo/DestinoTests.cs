using EVANS.Domain.Catalogo;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class DestinoTests
{
    [Fact]
    public void Crear_ValidData_SetsDistance()
    {
        var destino = Destino.Crear("LIMA", 12.5);

        destino.Descripcion.Should().Be("LIMA");
        destino.DistanciaVirtual.Should().Be(12.5);
    }

    [Fact]
    public void Crear_WithEstadoCodigo_PreservesSelectedStatus()
    {
        var destino = Destino.Crear("LIMA", 12.5, CatalogoEstado.Inactivo);

        destino.EstadoCodigo.Should().Be(CatalogoEstado.Inactivo);
    }

    [Fact]
    public void Crear_NegativeDistance_ThrowsDomainException()
    {
        Action act = () => Destino.Crear("LIMA", -1);

        act.Should().Throw<DomainException>().Where(e => e.Code == "CAT-DES-002");
    }
}
