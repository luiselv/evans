using EVANS.Domain.GuiaRemision;
using FluentAssertions;

namespace EVANS.Domain.Tests.GuiaRemision;

public class GuiaTests
{
    private static DetalleGuia ValidDetalle() =>
        new DetalleGuia(null, "Producto", new Peso(10m), 100m, 100m, 1);

    private static Guia ValidGuia(bool hasManifest = false, int? vehiculoId = null, int? carretaId = null, int? choferId = null) =>
        new Guia(
            codigo: null,
            numero: new NumeroGuia("T001", 1),
            fecha: DateTime.Today,
            remitenteId: 1,
            destinatarioId: 2,
            direccionPartida: new Direccion("Av Lima", "Lima", "Lima"),
            direccionLlegada: new Direccion("Jr Puno", "Puno", "Puno"),
            hasManifest: hasManifest,
            vehiculoId: vehiculoId,
            carretaId: carretaId,
            choferId: choferId,
            igv: 0.18m,
            detalles: new[] { ValidDetalle() }
        );

    [Fact]
    public void Constructor_NoDetalles_Throws()
    {
        Action act = () => new Guia(
            null, new NumeroGuia("T001", 1), DateTime.Today,
            1, 2,
            new Direccion("Av Lima", "Lima", "Lima"),
            new Direccion("Jr Puno", "Puno", "Puno"),
            false, null, null, null, 0.18m,
            Array.Empty<DetalleGuia>()
        );
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Constructor_RemitenteIdZero_Throws()
    {
        Action act = () => new Guia(
            null, new NumeroGuia("T001", 1), DateTime.Today,
            0, 2,
            new Direccion("Av Lima", "Lima", "Lima"),
            new Direccion("Jr Puno", "Puno", "Puno"),
            false, null, null, null, 0.18m,
            new[] { ValidDetalle() }
        );
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Constructor_DestinatarioIdZero_Throws()
    {
        Action act = () => new Guia(
            null, new NumeroGuia("T001", 1), DateTime.Today,
            1, 0,
            new Direccion("Av Lima", "Lima", "Lima"),
            new Direccion("Jr Puno", "Puno", "Puno"),
            false, null, null, null, 0.18m,
            new[] { ValidDetalle() }
        );
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Constructor_HasManifestTrueWithoutVehiculo_Throws()
    {
        Action act = () => ValidGuia(hasManifest: true, vehiculoId: null, carretaId: null, choferId: null);
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Constructor_HasManifestFalseWithoutVehiculo_Succeeds()
    {
        var guia = ValidGuia(hasManifest: false);
        guia.HasManifest.Should().BeFalse();
    }

    [Fact]
    public void ReemplazarDetalles_ReplacesCollection()
    {
        var guia = ValidGuia();
        var newDetalles = new[]
        {
            new DetalleGuia(null, "Nuevo Producto 1", new Peso(5m), 50m, 50m, 2),
            new DetalleGuia(null, "Nuevo Producto 2", new Peso(5m), 50m, 50m, 3)
        };

        guia.ReemplazarDetalles(newDetalles);

        guia.Detalles.Should().HaveCount(2);
        guia.Detalles[0].Descripcion.Should().Be("Nuevo Producto 1");
    }

    [Fact]
    public void CalcularIgv_AppliesFormula()
    {
        // PrecioTotal = 118, igvRate = 0.18 => IGV = 118 / (1 + 0.18) * 0.18 ≈ 18
        var detalle = new DetalleGuia(null, "Producto", new Peso(1m), 118m, 118m, 1);
        var guia = new Guia(
            null, new NumeroGuia("T001", 1), DateTime.Today,
            1, 2,
            new Direccion("Av Lima", "Lima", "Lima"),
            new Direccion("Jr Puno", "Puno", "Puno"),
            false, null, null, null, 0.18m,
            new[] { detalle }
        );

        var igv = guia.CalcularIgv(0.18m);
        // 118 / (1 + 0.18) = 100, IGV = 118 - 100 = 18
        igv.Should().BeApproximately(18m, 0.01m);
    }

    [Fact]
    public void Constructor_ValidData_Succeeds()
    {
        var guia = ValidGuia();
        guia.Detalles.Should().HaveCount(1);
        guia.RemitenteId.Should().Be(1);
        guia.DestinatarioId.Should().Be(2);
    }
}
