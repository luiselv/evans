using EVANS.Application.Manifiesto.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EVANS.Reports.Manifiesto;

/// <summary>
/// Renders a Manifiesto as PDF bytes using QuestPDF.
/// Groups Lineas by DestinoCodigo using ManifiestoDestinacionGrouper — no magic integers in rendering logic.
/// Emits per-destination subtotals and a grand total footer.
/// Receives a fully-hydrated ManifiestoDto — zero DB access.
/// </summary>
public sealed class ManifestoPdfRenderer
{
    private const string EvansBlue = "#1B3A8A";
    private const string White = "#FFFFFF";
    private const string LightGray = "#DDDDDD";
    private const string SubtotalBg = "#EEF2FF";

    static ManifestoPdfRenderer()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>Renders the manifiesto DTO to PDF bytes (sync — QuestPDF is sync-only).</summary>
    public byte[] Render(ManifiestoDto manifiesto)
    {
        var logoBytes = LoadLogo();

        // Group lineas by destination using the grouper — NO magic numbers in rendering logic
        var groups = ManifiestoDestinationGrouper.GroupByDestino(manifiesto.Lineas);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Content().Column(col =>
                {
                    // ---- Header ----
                    col.Item().Row(row =>
                    {
                        row.RelativeItem(3).Column(left =>
                        {
                            left.Item().Height(50).Image(logoBytes).FitArea();
                            left.Item().PaddingTop(4).Text("Of. Principal: Rossini 770, Urb. Primavera - Trujillo")
                                .FontSize(6);
                        });

                        row.ConstantItem(10);

                        row.RelativeItem(2).Border(1).BorderColor(EvansBlue).Padding(4).Column(right =>
                        {
                            right.Item().Text("MANIFIESTO DE CARGA").Bold().FontSize(12).FontColor(EvansBlue);
                            right.Item().PaddingTop(4).Text(manifiesto.Numero).Bold().FontSize(13).FontColor(EvansBlue);
                        });
                    });

                    col.Item().PaddingTop(6);

                    // ---- Manifiesto info ----
                    col.Item().Border(1).BorderColor(LightGray).Padding(4).Column(info =>
                    {
                        info.Item().Row(row =>
                        {
                            row.RelativeItem().Text(t =>
                            {
                                t.Span("Fecha: ").Bold();
                                t.Span(manifiesto.Fecha.ToString("dd/MM/yyyy"));
                            });
                            row.RelativeItem().Text(t =>
                            {
                                t.Span("Nro. Guías: ").Bold();
                                t.Span(manifiesto.NroGuias.ToString());
                            });
                        });

                        info.Item().PaddingTop(3).Row(row =>
                        {
                            row.RelativeItem().Text(t =>
                            {
                                t.Span("Transportista: ").Bold();
                                t.Span(manifiesto.TransportistaNombre);
                            });
                            row.RelativeItem().Text(t =>
                            {
                                t.Span("Vehículo: ").Bold();
                                t.Span(manifiesto.VehiculoPlaca);
                            });
                        });

                        info.Item().PaddingTop(3).Row(row =>
                        {
                            row.RelativeItem().Text(t =>
                            {
                                t.Span("Chofer: ").Bold();
                                t.Span(manifiesto.ChoferNombre);
                            });
                            if (manifiesto.CarretaPlaca is not null)
                            {
                                row.RelativeItem().Text(t =>
                                {
                                    t.Span("Carreta: ").Bold();
                                    t.Span(manifiesto.CarretaPlaca);
                                });
                            }
                            else
                            {
                                row.RelativeItem();
                            }
                        });
                    });

                    col.Item().PaddingTop(4);

                    // ---- Per-destination groups ----
                    foreach (var group in groups)
                    {
                        // Group header — destination name
                        col.Item().Background(EvansBlue).Padding(4)
                            .Text(group.DestinoNombre).FontColor(White).Bold().FontSize(10);

                        // Guia lines table
                        col.Item().Border(1).BorderColor(LightGray).Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(2);   // Numero guia
                                cols.RelativeColumn(1);   // Peso
                                cols.RelativeColumn(1);   // Flete
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(LightGray).Padding(3)
                                    .Text("GUÍA").Bold().FontSize(8);
                                header.Cell().Background(LightGray).Padding(3)
                                    .Text("PESO (kg)").Bold().FontSize(8).AlignRight();
                                header.Cell().Background(LightGray).Padding(3)
                                    .Text("FLETE (S/.)").Bold().FontSize(8).AlignRight();
                            });

                            foreach (var linea in group.Lineas)
                            {
                                table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                    .Text(linea.NumeroGuia);
                                table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                    .Text($"{linea.Peso:F2}").AlignRight();
                                table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                    .Text($"{linea.Flete:F2}").AlignRight();
                            }
                        });

                        // Subtotal row for this destination
                        col.Item().Background(SubtotalBg).Padding(4).Row(subtotal =>
                        {
                            subtotal.RelativeItem(2).Text($"Subtotal {group.DestinoNombre}:").Bold().FontSize(8);
                            subtotal.RelativeItem(1).Text($"{group.TotalPeso:F2}").Bold().FontSize(8).AlignRight();
                            subtotal.RelativeItem(1).Text($"{group.TotalFlete:F2}").Bold().FontSize(8).AlignRight();
                        });

                        col.Item().PaddingTop(4);
                    }

                    // ---- Grand total footer ----
                    var grandTotalFlete = manifiesto.Lineas.Sum(l => l.Flete);
                    var grandTotalPeso = manifiesto.Lineas.Sum(l => l.Peso);

                    col.Item().Border(1).BorderColor(EvansBlue).Padding(4).Row(row =>
                    {
                        row.RelativeItem(2).Text("IMPORTE TOTAL:").Bold().FontSize(11);
                        row.RelativeItem(1).Text($"{grandTotalPeso:F2} kg").Bold().FontSize(9).AlignRight();
                        row.RelativeItem(1).Text($"S/. {manifiesto.Importe:F2}").Bold().FontSize(11).AlignRight();
                    });
                });
            });
        });

        return document.GeneratePdf();
    }

    private static byte[] LoadLogo()
    {
        var assembly = typeof(ManifestoPdfRenderer).Assembly;
        const string name = "EVANS.Reports.Assets.logo.png";
        using var stream = assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException($"Embedded resource '{name}' not found.");
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}

/// <summary>
/// Groups ManifiestoLineaDto list by DestinoCodigo.
/// Using a dedicated class avoids magic integers in the renderer — R-03 constraint.
/// </summary>
public static class ManifiestoDestinationGrouper
{
    /// <summary>Returns lineas grouped by DestinoCodigo, preserving insertion order of first occurrence.</summary>
    public static IReadOnlyList<DestinoGroup> GroupByDestino(IReadOnlyList<ManifiestoLineaDto> lineas)
    {
        return lineas
            .GroupBy(l => l.DestinoCodigo)
            .Select(g => new DestinoGroup(
                DestinoCodigo: g.Key,
                DestinoNombre: g.First().DestinoNombre,
                Lineas: g.ToList(),
                TotalPeso: g.Sum(l => l.Peso),
                TotalFlete: g.Sum(l => l.Flete)))
            .ToList();
    }
}

/// <summary>Represents a group of guias destined for the same destination.</summary>
public sealed record DestinoGroup(
    int DestinoCodigo,
    string DestinoNombre,
    IReadOnlyList<ManifiestoLineaDto> Lineas,
    decimal TotalPeso,
    decimal TotalFlete);
