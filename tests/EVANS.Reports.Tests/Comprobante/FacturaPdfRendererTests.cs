using EVANS.Application.Comprobante.DTOs;
using EVANS.Domain.Comprobante;
using EVANS.Reports.Comprobante;
using UglyToad.PdfPig;

namespace EVANS.Reports.Tests.Comprobante;

[UsesVerify]
public class FacturaPdfRendererTests
{
    private static readonly FacturaPdfRenderer Renderer = new();

    private static ComprobanteDto BuildFacturaDto() => new(
        Codigo: 1,
        NumeroComprobante: "F001-000001",
        Tipo: TipoComprobante.Factura,
        Fecha: new DateTime(2024, 6, 15),
        ClienteCodigo: 1,
        RucODni: "20123456789",
        Direccion: "Av Lima 123|Lima|Lima",
        Total: 118.00m,
        IGV: 18.00m,
        ValorVenta: 100.00m,
        Impreso: false,
        Detalles:
        [
            new DetalleComprobanteDto(1, "Servicio de transporte Factura", 100m, 118m)
        ]);

    // ------------------------------------------------------------------
    // (a) Returns non-empty bytes starting with %PDF
    // ------------------------------------------------------------------

    [Fact]
    public void Render_Factura_ReturnsPdfBytes()
    {
        var dto = BuildFacturaDto();

        var bytes = Renderer.Render(dto);

        bytes.Should().NotBeNullOrEmpty();
        bytes.Length.Should().BeGreaterThan(1000);
        System.Text.Encoding.ASCII.GetString(bytes[..4]).Should().Be("%PDF");
    }

    // ------------------------------------------------------------------
    // (b) PDF contains IGV section and RUC
    // ------------------------------------------------------------------

    [Fact]
    public void Render_Factura_ContainsIgvSection()
    {
        var dto = BuildFacturaDto();

        var bytes = Renderer.Render(dto);
        var text = ExtractText(bytes);

        text.Should().Contain("IGV");
        text.Should().Contain("20123456789");   // RUC
    }

    // ------------------------------------------------------------------
    // (c) IGV math is correct: Total=118, ValorVenta=100.00, IGV=18.00
    // ------------------------------------------------------------------

    [Fact]
    public void Render_Factura_IgvMath_Correct()
    {
        var dto = BuildFacturaDto();

        var bytes = Renderer.Render(dto);
        var text = ExtractText(bytes);

        text.Should().Contain("100.00");   // ValorVenta
        text.Should().Contain("18.00");    // IGV
        text.Should().Contain("118.00");   // Total
    }

    // ------------------------------------------------------------------
    // (d) Verify snapshot
    // ------------------------------------------------------------------

    [Fact]
    public Task Render_Factura_Snapshot()
    {
        var text = ExtractText(Renderer.Render(BuildFacturaDto()));
        return Verify(text).UseMethodName(nameof(Render_Factura_Snapshot));
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static string ExtractText(byte[] pdf)
    {
        using var doc = PdfDocument.Open(pdf);
        var pages = doc.GetPages().Select((p, i) =>
            $"=== Page {i + 1} ===\n{p.Text.Trim()}");
        return string.Join("\n\n", pages);
    }
}
