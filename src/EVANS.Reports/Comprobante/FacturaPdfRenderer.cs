using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Ports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EVANS.Reports.Comprobante;

/// <summary>
/// Renders a Factura (invoice with IGV) as PDF bytes using QuestPDF.
/// Includes RUC, ValorVenta, IGV%, IGV amount, and Total footer sections.
/// </summary>
public sealed class FacturaPdfRenderer : IDocumentPrinter
{
    private const string EvansBlue = "#1B3A8A";
    private const string White = "#FFFFFF";
    private const string LightGray = "#DDDDDD";

    static FacturaPdfRenderer()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] Render(ComprobanteDto comprobante)
    {
        var logoBytes = LoadLogo();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Content().Column(col =>
                {
                    // Section 1 — Header
                    col.Item().Row(row =>
                    {
                        // LEFT: logo + RUC
                        row.RelativeItem(3).Column(left =>
                        {
                            left.Item().Height(50).Image(logoBytes).FitArea();
                            left.Item().PaddingTop(4).Text("Of. Principal: Rossini 770, Urb. Primavera - Trujillo")
                                .FontSize(6);
                            left.Item().PaddingTop(2).Text(t =>
                            {
                                t.Span("RUC: ").Bold().FontSize(8);
                                t.Span("20440309853").FontSize(8);
                            });
                        });

                        row.ConstantItem(10);

                        // RIGHT: comprobante type box
                        row.RelativeItem(2).Border(1).BorderColor(EvansBlue).Padding(4).Column(right =>
                        {
                            right.Item().Text("FACTURA").Bold().FontSize(12).FontColor(EvansBlue);
                            right.Item().PaddingTop(4)
                                .Text(comprobante.NumeroComprobante).Bold().FontSize(13).FontColor(EvansBlue);
                        });
                    });

                    col.Item().PaddingTop(6);

                    // Section 2 — Fecha + Cliente + RUC Cliente
                    col.Item().Border(1).BorderColor(LightGray).Padding(4).Column(info =>
                    {
                        info.Item().Row(row =>
                        {
                            row.RelativeItem().Text(t =>
                            {
                                t.Span("Fecha: ").Bold();
                                t.Span(comprobante.Fecha.ToString("dd/MM/yyyy"));
                            });
                        });

                        info.Item().PaddingTop(3).Text(t =>
                        {
                            t.Span("Señor(es): ").Bold();
                            t.Span(comprobante.ClienteCodigo.ToString());
                        });

                        info.Item().PaddingTop(2).Text(t =>
                        {
                            t.Span("RUC: ").Bold();
                            t.Span(comprobante.RucODni);
                        });

                        if (!string.IsNullOrWhiteSpace(comprobante.Direccion))
                        {
                            var addr = ParseDireccion(comprobante.Direccion);
                            info.Item().PaddingTop(2).Text(t =>
                            {
                                t.Span("Dirección: ").Bold();
                                t.Span(addr);
                            });
                        }
                    });

                    col.Item().PaddingTop(4);

                    // Section 3 — Detalles table
                    col.Item().Border(1).BorderColor(EvansBlue).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(50);   // CANTIDAD
                            cols.RelativeColumn();     // DESCRIPCION
                            cols.ConstantColumn(80);   // IMPORTE
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(EvansBlue).Padding(4)
                                .Text("CANTIDAD").FontColor(White).Bold().FontSize(8).AlignCenter();
                            header.Cell().Background(EvansBlue).Padding(4)
                                .Text("DESCRIPCION").FontColor(White).Bold().FontSize(8).AlignCenter();
                            header.Cell().Background(EvansBlue).Padding(4)
                                .Text("IMPORTE").FontColor(White).Bold().FontSize(8).AlignCenter();
                        });

                        foreach (var d in comprobante.Detalles)
                        {
                            table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                .Text(d.Cantidad.ToString()).AlignCenter();
                            table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                .Text(d.Descripcion);
                            table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                .Text($"S/. {d.Flete:F2}").AlignRight();
                        }

                        if (!comprobante.Detalles.Any())
                        {
                            table.Cell().ColumnSpan(3).Padding(10).Text(" ");
                        }
                    });

                    col.Item().PaddingTop(4);

                    // Section 4 — IGV footer: ValorVenta, IGV, Total
                    col.Item().Border(1).BorderColor(LightGray).Padding(4).Column(footer =>
                    {
                        footer.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Valor de Venta:").FontSize(9);
                            row.ConstantItem(100)
                                .Text($"S/. {comprobante.ValorVenta:F2}")
                                .FontSize(9)
                                .AlignRight();
                        });

                        footer.Item().PaddingTop(2).Row(row =>
                        {
                            row.RelativeItem().Text("IGV (18%):").FontSize(9);
                            row.ConstantItem(100)
                                .Text($"S/. {comprobante.IGV:F2}")
                                .FontSize(9)
                                .AlignRight();
                        });

                        footer.Item().PaddingTop(4).Row(row =>
                        {
                            row.RelativeItem().Text("TOTAL:").Bold().FontSize(11);
                            row.ConstantItem(100)
                                .Text($"S/. {comprobante.Total:F2}")
                                .Bold()
                                .FontSize(11)
                                .AlignRight();
                        });
                    });
                });
            });
        });

        return document.GeneratePdf();
    }

    private static string ParseDireccion(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return string.Empty;
        var parts = raw.Split('|');
        return parts[0];
    }

    private static byte[] LoadLogo()
    {
        var assembly = typeof(FacturaPdfRenderer).Assembly;
        const string name = "EVANS.Reports.Assets.logo.png";
        using var stream = assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException($"Embedded resource '{name}' not found.");
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
