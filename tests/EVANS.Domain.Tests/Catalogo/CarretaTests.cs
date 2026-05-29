using EVANS.Domain.Catalogo;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class CarretaTests
{
    [Fact]
    public void Crear_NormalizesPlacaAndKeepsEmpresa()
    {
        var carreta = Carreta.Crear("abc-999", "Randon", "CERT", 10);

        carreta.Placa.Should().Be("ABC-999");
        carreta.EmpresaCodigo.Should().Be(10);
    }

    [Fact]
    public void Crear_WithoutPlaca_ThrowsDomainException()
    {
        Action act = () => Carreta.Crear(" ", null, null, 10);

        act.Should().Throw<DomainException>().Where(e => e.Code == "CAT-CAR-001");
    }
}
