using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Reports.GuiaRemision;
using UglyToad.PdfPig;

namespace EVANS.Reports.Tests.GuiaRemision;

[UsesVerify]
public class GuiaPdfRendererSnapshotTests
{
    private static readonly GuiaPdfRenderer Renderer = new();

    // -- Fixture builders --

    private static GuiaDetalleDto Minimal() => new(
        Codigo: 1,
        NumeroGuia: "GREM-000001",
        Fecha: new DateTime(2024, 3, 15),
        RemitenteId: 1,
        RemitenteNombre: "WILLY BUSCH S.C.R.L.",
        DestinatarioId: 2,
        DestinatarioNombre: "AUTOMOTRIZ ROYAL E.I.R.L.",
        DireccionPartida: "Av. Venezuela 1908|Lima|Lima",
        DireccionLlegada: "Av. España 1800|Trujillo|La Libertad",
        HasManifest: false,
        VehiculoId: null, CarretaId: null, ChoferId: null,
        Igv: 0.18m,
        Detalles: [
            new(null, "Carga general - repuestos", 23.50m, 150m, 150m, 1)
        ]);

    private static GuiaDetalleDto ManyDetalles() => new(
        Codigo: 2,
        NumeroGuia: "GREM-000002",
        Fecha: new DateTime(2024, 3, 15),
        RemitenteId: 1,
        RemitenteNombre: "MOVILCAR S.R.L.",
        DestinatarioId: 3,
        DestinatarioNombre: "SERVISOL LUBRICACION S.A.C.",
        DireccionPartida: "Calle Los Pinos 234|Lima|Lima",
        DireccionLlegada: "Jr. Independencia 567|Chiclayo|Lambayeque",
        HasManifest: false,
        VehiculoId: null, CarretaId: null, ChoferId: null,
        Igv: 0.18m,
        Detalles: [
            new(null, "Filtros de aceite", 5.00m, 45m, 45m, 1),
            new(null, "Filtros de aire", 3.50m, 38m, 38m, 1),
            new(null, "Bujías NGK", 1.20m, 25m, 50m, 2),
            new(null, "Aceite 20W-50 galón", 8.00m, 85m, 255m, 3),
            new(null, "Pastillas de freno delanteras", 4.50m, 120m, 120m, 1),
            new(null, "Pastillas de freno traseras", 3.80m, 95m, 95m, 1),
            new(null, "Correa de distribución", 2.10m, 180m, 180m, 1),
            new(null, "Amortiguador delantero", 12.00m, 250m, 500m, 2),
        ]);

    private static GuiaDetalleDto ConManifiesto() => new(
        Codigo: 3,
        NumeroGuia: "GREM-000003",
        Fecha: new DateTime(2024, 3, 15),
        RemitenteId: 4,
        RemitenteNombre: "AUTOREX PERUANA S.A.",
        DestinatarioId: 5,
        DestinatarioNombre: "REPUESTOS AUTOMOTRIZ ELIZABETH E.I.R.L.",
        DireccionPartida: "Av. Argentina 3093|Lima|Lima",
        DireccionLlegada: "Jr. Bolívar 890|Trujillo|La Libertad",
        HasManifest: true,
        VehiculoId: 1, CarretaId: null, ChoferId: 1,
        Igv: 0.18m,
        Detalles: [
            new(null, "Kit de embrague completo", 18.00m, 320m, 320m, 1),
            new(null, "Bomba de agua", 7.50m, 145m, 145m, 1),
        ]);

    private static GuiaDetalleDto ConVehiculoCompleto() => new(
        Codigo: 4,
        NumeroGuia: "GREM-000004",
        Fecha: new DateTime(2024, 3, 15),
        RemitenteId: 6,
        RemitenteNombre: "INDUSTRIAS WILLY BUSCH S.A.",
        DestinatarioId: 7,
        DestinatarioNombre: "OLEOCENTRO SANDY S.R.L.",
        DireccionPartida: "Av. Venezuela 1908 Cercado|Lima|Lima",
        DireccionLlegada: "Av. Mansiche 1450|Trujillo|La Libertad",
        HasManifest: false,
        VehiculoId: 2, CarretaId: 1, ChoferId: 3,
        Igv: 0.18m,
        Detalles: [
            new(null, "Aceite de motor Castrol 15W-40", 16.00m, 72m, 216m, 3),
            new(null, "Grasa de chasis kg", 5.00m, 18m, 36m, 2),
        ]);

    private static GuiaDetalleDto NombresLargos() => new(
        Codigo: 5,
        NumeroGuia: "GREM-000005",
        Fecha: new DateTime(2024, 3, 15),
        RemitenteId: 8,
        RemitenteNombre: "MAC JOHNSON CONTROLS COLOMBIA S.A.S. SUCURSAL PERU",
        DestinatarioId: 9,
        DestinatarioNombre: "CORPORACION DISTRIBUIDORA AUTOMOTRIZ LA SOLUCION INTEGRAL S.A.C.",
        DireccionPartida: "Calle Los Cipreses de Pro Mz F Lote 12 Urbanizacion Industrial|Lima|Lima",
        DireccionLlegada: "Av. Cesar Vallejo 1850 Urb. La Merced|Trujillo|La Libertad",
        HasManifest: false,
        VehiculoId: null, CarretaId: null, ChoferId: null,
        Igv: 0.18m,
        Detalles: [
            new(null, "Batería de arranque 12V 65Ah libre de mantenimiento", 28.00m, 485m, 485m, 1),
        ]);

    // -- Helpers --

    private static string ExtractText(byte[] pdf)
    {
        using var doc = PdfDocument.Open(pdf);
        var pages = doc.GetPages().Select((p, i) =>
            $"=== Page {i + 1} ===\n{p.Text.Trim()}");
        return string.Join("\n\n", pages);
    }

    // -- Tests --

    [Fact]
    public Task Snapshot_Minimal_OneDetalle()
    {
        var text = ExtractText(Renderer.Generate(Minimal()));
        return Verify(text).UseMethodName(nameof(Snapshot_Minimal_OneDetalle));
    }

    [Fact]
    public Task Snapshot_ManyDetalles_EightItems()
    {
        var text = ExtractText(Renderer.Generate(ManyDetalles()));
        return Verify(text).UseMethodName(nameof(Snapshot_ManyDetalles_EightItems));
    }

    [Fact]
    public Task Snapshot_HasManifest_True()
    {
        var text = ExtractText(Renderer.Generate(ConManifiesto()));
        return Verify(text).UseMethodName(nameof(Snapshot_HasManifest_True));
    }

    [Fact]
    public Task Snapshot_VehiculoCompleto_AllIds()
    {
        var text = ExtractText(Renderer.Generate(ConVehiculoCompleto()));
        return Verify(text).UseMethodName(nameof(Snapshot_VehiculoCompleto_AllIds));
    }

    [Fact]
    public Task Snapshot_NombresLargos_Truncation()
    {
        var text = ExtractText(Renderer.Generate(NombresLargos()));
        return Verify(text).UseMethodName(nameof(Snapshot_NombresLargos_Truncation));
    }
}
