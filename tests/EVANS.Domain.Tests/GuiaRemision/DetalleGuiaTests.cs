using EVANS.Domain.GuiaRemision;
using FluentAssertions;

namespace EVANS.Domain.Tests.GuiaRemision;

public class DetalleGuiaTests
{
    [Fact]
    public void Constructor_EmptyDescripcion_Throws()
    {
        Action act = () => new DetalleGuia(null, "", new Peso(1m), 10m, 10m, 1);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_CantidadZero_Throws()
    {
        Action act = () => new DetalleGuia(null, "Producto", new Peso(1m), 10m, 10m, 0);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NegativePrecio_Throws()
    {
        Action act = () => new DetalleGuia(null, "Producto", new Peso(1m), -1m, 10m, 1);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_ValidData_Succeeds()
    {
        var detalle = new DetalleGuia(null, "Producto", new Peso(5m), 100m, 100m, 2);
        detalle.Descripcion.Should().Be("Producto");
        detalle.Cantidad.Should().Be(2);
        detalle.PrecioUnitario.Should().Be(100m);
    }
}
