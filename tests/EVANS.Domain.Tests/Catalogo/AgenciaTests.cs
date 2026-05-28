using EVANS.Domain.Catalogo;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class AgenciaTests
{
    [Fact]
    public void Materializar_ValidData_ReflectsSchemaColumns()
    {
        var agencia = Agencia.Materializar(1, "Av Lima", 10, CatalogoEstado.Activo);

        agencia.Codigo.Should().Be(1);
        agencia.Direccion.Should().Be("Av Lima");
        agencia.DestinoCodigo.Should().Be(10);
        agencia.EstadoCodigo.Should().Be(CatalogoEstado.Activo);
    }

    [Fact]
    public void Materializar_WithoutDireccion_ThrowsDomainException()
    {
        Action act = () => Agencia.Materializar(1, " ", 10, CatalogoEstado.Activo);

        act.Should().Throw<DomainException>().Where(e => e.Code == "CAT-AGE-001");
    }
}
