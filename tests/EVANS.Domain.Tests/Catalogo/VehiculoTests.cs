using EVANS.Domain.Catalogo;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class VehiculoTests
{
    [Fact]
    public void Crear_NormalizesPlacaToUppercase()
    {
        var vehiculo = Vehiculo.Crear("Volvo", "abc-123", "C2", "CERT", 10);

        vehiculo.Placa.Should().Be("ABC-123");
        vehiculo.EstadoCodigo.Should().Be(CatalogoEstado.Activo);
    }

    [Fact]
    public void Crear_WithoutConfiguracion_ThrowsDomainException()
    {
        Action act = () => Vehiculo.Crear("Volvo", "ABC-123", " ", null, 10);

        act.Should().Throw<DomainException>().Where(e => e.Code == "CAT-VEH-002");
    }
}
