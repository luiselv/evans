using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Reports.GuiaRemision;

namespace EVANS.Reports.Tests.GuiaRemision;

public class GuiaPdfRendererTests
{
    private static GuiaDetalleDto BuildTestDto() => new(
        Codigo: 1,
        NumeroGuia: "GREM-000001",
        Fecha: new DateTime(2024, 1, 15),
        RemitenteId: 1,
        RemitenteNombre: "WILLY BUSCH S.C.R.L.",
        DestinatarioId: 2,
        DestinatarioNombre: "AUTOMOTRIZ ROYAL E.I.R.L.",
        DireccionPartida: "Av Lima 123|Lima|Lima",
        DireccionLlegada: "Av Trujillo 456|Trujillo|La Libertad",
        HasManifest: false,
        VehiculoId: null,
        CarretaId: null,
        ChoferId: null,
        Igv: 0.18m,
        Detalles: new List<DetalleGuiaItemDto>
        {
            new(null, "Caja de repuestos automotrices", 23.73m, 100m, 100m, 1),
            new(null, "Filtros de aceite", 5.50m, 50m, 100m, 2)
        });

    [Fact]
    public void Generate_ValidDto_ReturnsPdfBytes()
    {
        var renderer = new GuiaPdfRenderer();
        var dto = BuildTestDto();

        var bytes = renderer.Generate(dto);

        bytes.Should().NotBeNullOrEmpty();
        bytes.Length.Should().BeGreaterThan(1000);
        System.Text.Encoding.ASCII.GetString(bytes[..4]).Should().Be("%PDF");
    }

    [Fact]
    public void Generate_EmptyDetalles_DoesNotThrow()
    {
        var renderer = new GuiaPdfRenderer();
        var dto = BuildTestDto() with { Detalles = new List<DetalleGuiaItemDto>() };

        var act = () => renderer.Generate(dto);

        act.Should().NotThrow();
    }
}
