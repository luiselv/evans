using FluentAssertions;
using Xunit;
using Det = EVANS.Domain.Recepcion.DetalleRecepcion;
using DomEx = EVANS.Domain.Recepcion.DomainException;

namespace EVANS.Domain.Tests.Recepcion;

public class DetalleRecepcionTests
{
    private static Det ValidDetalle() =>
        Det.Crear(2m, "Carga general", 10m, "KG", 100m, "GR", "001-000001");

    // D-07: DRE001 — Cantidad <= 0
    [Fact]
    public void Crear_CantidadCero_ThrowsDRE001()
    {
        var act = () => Det.Crear(0m, "Carga", 10m, "KG", 100m, "GR", "001");

        act.Should().Throw<DomEx>().Which.Code.Should().Be("DRE001");
    }

    // D-08: DRE002 — Descripcion vacía
    [Fact]
    public void Crear_DescripcionVacia_ThrowsDRE002()
    {
        var act = () => Det.Crear(1m, "", 10m, "KG", 100m, "GR", "001");

        act.Should().Throw<DomEx>().Which.Code.Should().Be("DRE002");
    }

    // D-09: DRE003 — Costo negativo
    [Fact]
    public void Crear_CostoNegativo_ThrowsDRE003()
    {
        var act = () => Det.Crear(1m, "Carga", 10m, "KG", -1m, "GR", "001");

        act.Should().Throw<DomEx>().Which.Code.Should().Be("DRE003");
    }

    // DRE004 — Peso negativo
    [Fact]
    public void Crear_PesoNegativo_ThrowsDRE004()
    {
        var act = () => Det.Crear(1m, "Carga", -0.1m, "KG", 100m, "GR", "001");

        act.Should().Throw<DomEx>().Which.Code.Should().Be("DRE004");
    }

    // D-10: CalcularCostoSinIgv correcto — Costo=118, tasa=0.18 → 100
    [Fact]
    public void CalcularCostoSinIgv_ValorCorrecto()
    {
        var detalle = Det.Crear(1m, "Test", 0m, "UND", 118m, "GR", "001");

        var resultado = detalle.CalcularCostoSinIgv(0.18m);

        resultado.Should().Be(100m);
    }

    // D-11: IGV01 — tasaIgv negativa
    [Fact]
    public void CalcularCostoSinIgv_TasaNegativa_ThrowsIGV01()
    {
        var detalle = ValidDetalle();

        var act = () => detalle.CalcularCostoSinIgv(-0.01m);

        act.Should().Throw<DomEx>().Which.Code.Should().Be("IGV01");
    }

    // Edge case: tasaIgv = 0 → costo unchanged
    [Fact]
    public void CalcularCostoSinIgv_TasaCero_ReturnsCostoOriginal()
    {
        var detalle = Det.Crear(1m, "Test", 0m, "UND", 200m, "GR", "001");

        var resultado = detalle.CalcularCostoSinIgv(0m);

        resultado.Should().Be(200m);
    }

    // AjustarCostoSinIgv mutates Costo
    [Fact]
    public void AjustarCostoSinIgv_MutatesCosto()
    {
        var detalle = Det.Crear(1m, "Test", 0m, "UND", 118m, "GR", "001");

        detalle.AjustarCostoSinIgv(0.18m);

        detalle.Costo.Should().Be(100m);
    }
}
