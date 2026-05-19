using EVANS.Application.Comprobante.DTOs;
using EVANS.Domain.Comprobante;
using EVANS.Reports.Comprobante;
using UglyToad.PdfPig;

namespace EVANS.Reports.Tests.Comprobante;

[UsesVerify]
public class BoletaPdfRendererTests
{
    private static readonly BoletaPdfRenderer Renderer = new();

    private static ComprobanteDto BuildBoletaDto() => new(
        Codigo: 1,
        NumeroComprobante: "B001-000001",
        Tipo: TipoComprobante.Boleta,
        Fecha: new DateTime(2024, 6, 15),
        ClienteCodigo: 1,
        RucODni: "12345678",
        Direccion: "Av Lima 123|Lima|Lima",
        Total: 50.00m,
        IGV: 0m,
        ValorVenta: 0m,
        Impreso: false,
        Detalles:
        [
            new DetalleComprobanteDto(1, "Servicio de transporte Boleta", 50m, 50m)
        ]);

    // ------------------------------------------------------------------
    // (a) Returns non-empty bytes starting with %PDF
    // ------------------------------------------------------------------

    [Fact]
    public void Render_Boleta_ReturnsPdfBytes()
    {
        var dto = BuildBoletaDto();

        var bytes = Renderer.Render(dto);

        bytes.Should().NotBeNullOrEmpty();
        bytes.Length.Should().BeGreaterThan(1000);
        System.Text.Encoding.ASCII.GetString(bytes[..4]).Should().Be("%PDF");
    }

    // ------------------------------------------------------------------
    // (b) PDF contains required fields — DNI, Total, Descripcion
    // ------------------------------------------------------------------

    [Fact]
    public void Render_Boleta_ContainsRequiredFields()
    {
        var dto = BuildBoletaDto();

        var bytes = Renderer.Render(dto);
        var text = ExtractText(bytes);

        text.Should().Contain("12345678");       // DNI
        text.Should().Contain("50");             // Total (S/. 50.00)
        text.Should().Contain("Servicio de transporte Boleta");  // Descripcion
    }

    // ------------------------------------------------------------------
    // (c) Boleta does NOT contain IGV or ValorVenta sections
    // ------------------------------------------------------------------

    [Fact]
    public void Render_Boleta_NoIgvSection()
    {
        var dto = BuildBoletaDto();

        var bytes = Renderer.Render(dto);
        var text = ExtractText(bytes);

        text.Should().NotContain("IGV");
        text.Should().NotContain("Valor de Venta");
        text.Should().NotContain("Valor Venta");
    }

    // ------------------------------------------------------------------
    // (d) Verify snapshot — extract text and snapshot
    // ------------------------------------------------------------------

    [Fact]
    public Task Render_Boleta_Snapshot()
    {
        var text = ExtractText(Renderer.Render(BuildBoletaDto()));
        return Verify(text).UseMethodName(nameof(Render_Boleta_Snapshot));
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
