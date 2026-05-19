using EVANS.Domain.Comprobante;
using FluentAssertions;
using Agg = EVANS.Domain.Comprobante.Comprobante;

namespace EVANS.Domain.Tests.Comprobante;

public class ComprobanteInvariantsTests
{
    private static NumeroComprobante FakeNumeroFactura() => new("F001", "000001");
    private static NumeroComprobante FakeNumeroBoleta() => new("B001", "000001");

    private static IReadOnlyList<DetalleComprobante> OneDetalle() =>
        new List<DetalleComprobante>
        {
            new(1, "Flete Lima-Arequipa", 100m, 118m)
        };

    // --- Throw on empty detalles ---

    [Fact]
    public void CrearFactura_EmptyDetalles_Throws()
    {
        var action = () => Agg.CrearFactura(
            FakeNumeroFactura(), DateTime.Today, 1, "20123456789", "Av Lima",
            new List<DetalleComprobante>(), new Standalone(), 0.18m);

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void CrearBoleta_EmptyDetalles_Throws()
    {
        var action = () => Agg.CrearBoleta(
            FakeNumeroBoleta(), DateTime.Today, 1, "12345678", "Av Lima",
            new List<DetalleComprobante>(), new Standalone());

        action.Should().Throw<DomainException>();
    }

    // --- Throw on Total <= 0 ---

    [Fact]
    public void CrearFactura_ZeroFlete_Throws()
    {
        var detalles = new List<DetalleComprobante>
        {
            new(1, "Flete gratis", 0m, 0m)
        };

        var action = () => Agg.CrearFactura(
            FakeNumeroFactura(), DateTime.Today, 1, "20123456789", "Av Lima",
            detalles, new Standalone(), 0.18m);

        action.Should().Throw<DomainException>();
    }

    // --- Boleta: IGV == 0, ValorVenta == 0 ---

    [Fact]
    public void CrearBoleta_IgvIsZero()
    {
        var comprobante = Agg.CrearBoleta(
            FakeNumeroBoleta(), DateTime.Today, 1, "12345678", "Av Lima",
            OneDetalle(), new Standalone());

        comprobante.IGV.Should().Be(0m);
    }

    [Fact]
    public void CrearBoleta_ValorVentaIsZero()
    {
        var comprobante = Agg.CrearBoleta(
            FakeNumeroBoleta(), DateTime.Today, 1, "12345678", "Av Lima",
            OneDetalle(), new Standalone());

        comprobante.ValorVenta.Should().Be(0m);
    }

    // --- Factura: IGV and ValorVenta calculated ---

    [Fact]
    public void CrearFactura_IgvCalculatedCorrectly()
    {
        var comprobante = Agg.CrearFactura(
            FakeNumeroFactura(), DateTime.Today, 1, "20123456789", "Av Lima",
            OneDetalle(), new Standalone(), 0.18m);

        // Total = 118, rate = 0.18 => IGV = Round(118 * 0.18 / 1.18, 2) = Round(18.0, 2) = 18
        comprobante.IGV.Should().Be(18.00m);
    }

    [Fact]
    public void CrearFactura_ValorVentaCalculatedCorrectly()
    {
        var comprobante = Agg.CrearFactura(
            FakeNumeroFactura(), DateTime.Today, 1, "20123456789", "Av Lima",
            OneDetalle(), new Standalone(), 0.18m);

        // ValorVenta = Total - IGV = 118 - 18 = 100
        comprobante.ValorVenta.Should().Be(100.00m);
    }

    // --- Factura requires non-empty RUC ---

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void CrearFactura_EmptyRuc_Throws(string? ruc)
    {
        var action = () => Agg.CrearFactura(
            FakeNumeroFactura(), DateTime.Today, 1, ruc!, "Av Lima",
            OneDetalle(), new Standalone(), 0.18m);

        action.Should().Throw<DomainException>();
    }

    // --- Boleta requires non-empty DNI ---

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void CrearBoleta_EmptyDni_Throws(string? dni)
    {
        var action = () => Agg.CrearBoleta(
            FakeNumeroBoleta(), DateTime.Today, 1, dni!, "Av Lima",
            OneDetalle(), new Standalone());

        action.Should().Throw<DomainException>();
    }

    // --- DesdeGuia requires non-null/empty GuiaRef (invariant #6) ---

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void DesdeGuia_EmptyGuiaRef_Throws(string? guiaRef)
    {
        var action = () => new DesdeGuia(guiaRef!);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void DesdeGuia_ValidGuiaRef_DoesNotThrow()
    {
        var action = () => new DesdeGuia("F001-000001");
        action.Should().NotThrow();
    }

    // --- NumeroComprobante is immutable after creation (invariant #7) ---

    [Fact]
    public void NumeroComprobante_IsImmutable_RecordEquality()
    {
        var a = new NumeroComprobante("F001", "000042");
        var b = new NumeroComprobante("F001", "000042");
        // Record equality works and values cannot be changed
        a.Should().Be(b);
        // Verify properties are init-only (no setter)
        var type = typeof(NumeroComprobante);
        var serieProp = type.GetProperty(nameof(NumeroComprobante.Serie));
        serieProp!.SetMethod.Should().BeNull(); // no public setter
    }

    // --- Codigo is null before repository sets it ---

    [Fact]
    public void CrearFactura_CodigoIsNull()
    {
        var comprobante = Agg.CrearFactura(
            FakeNumeroFactura(), DateTime.Today, 1, "20123456789", "Av Lima",
            OneDetalle(), new Standalone(), 0.18m);

        comprobante.Codigo.Should().BeNull();
    }

    [Fact]
    public void CrearBoleta_CodigoIsNull()
    {
        var comprobante = Agg.CrearBoleta(
            FakeNumeroBoleta(), DateTime.Today, 1, "12345678", "Av Lima",
            OneDetalle(), new Standalone());

        comprobante.Codigo.Should().BeNull();
    }
}
