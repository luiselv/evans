using EVANS.Application.GuiaRemision.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EVANS.Reports.GuiaRemision;

public class GuiaPdfRenderer
{
    private const string EvansBlue = "#1B3A8A";
    private const string White = "#FFFFFF";
    private const string LightGray = "#DDDDDD";

    static GuiaPdfRenderer()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] Generate(GuiaDetalleDto dto)
    {
        var logoBytes = LoadLogo();
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(8));

                page.Content().Column(col =>
                {
                    // Section 1 — Header
                    col.Item().Row(row =>
                    {
                        // LEFT: logo + address lines
                        row.RelativeItem(3).Column(left =>
                        {
                            left.Item().Height(50).Image(logoBytes).FitArea();
                            left.Item().PaddingTop(4).Text("Of. Principal: Rossini 770, Urb. Primavera - Telefax(044) 260665 - Trujillo - La Libertad")
                                .FontSize(6);
                            left.Item().Text("Of. Sucursal Calle Delta 226 - Urb. Parque Internacional - Callao - Telefax 01- 452-2350 - Lima")
                                .FontSize(6);
                        });

                        row.ConstantItem(10); // spacer

                        // RIGHT: bordered RUC box
                        row.RelativeItem(2).Border(1).BorderColor(EvansBlue).Padding(4).Column(right =>
                        {
                            right.Item().Text("RUC Nº 20440309853").Bold().FontSize(8);
                            right.Item().Text("GUIA DE REMISION").Bold().FontSize(11).FontColor(EvansBlue);
                            right.Item().Text("TRANSPORTISTA").Bold().FontSize(11).FontColor(EvansBlue);
                            right.Item().Text("Registro Nº 131730 CNG").FontSize(7);
                            right.Item().PaddingTop(4).Text(FormatGuiaNumber(dto.NumeroGuia)).Bold().FontSize(13).FontColor(EvansBlue);
                        });
                    });

                    col.Item().PaddingTop(6);

                    // Section 2 — Fechas
                    col.Item().Border(1).BorderColor(LightGray).Padding(4).Row(row =>
                    {
                        row.RelativeItem().Text(t =>
                        {
                            t.Span("Fecha de Emisión: ").Bold();
                            t.Span(dto.Fecha.ToString("dd/MM/yyyy"));
                        });
                        row.RelativeItem().Text(t =>
                        {
                            t.Span("Fecha Inicio Traslado: ").Bold();
                            t.Span(dto.Fecha.ToString("dd/MM/yyyy"));
                        });
                    });

                    col.Item().PaddingTop(4);

                    // Section 3 — Directions
                    col.Item().Row(row =>
                    {
                        var partida = ParseDireccion(dto.DireccionPartida);
                        var llegada = ParseDireccion(dto.DireccionLlegada);

                        row.RelativeItem().Border(1).BorderColor(EvansBlue).Column(box =>
                        {
                            box.Item().Background(EvansBlue).Padding(3)
                                .Text("DIRECCION DE PARTIDA").FontColor(White).Bold().FontSize(7);
                            box.Item().Padding(4).Text(partida).FontSize(8);
                        });

                        row.ConstantItem(4);

                        row.RelativeItem().Border(1).BorderColor(EvansBlue).Column(box =>
                        {
                            box.Item().Background(EvansBlue).Padding(3)
                                .Text("DIRECCION DE LLEGADA").FontColor(White).Bold().FontSize(7);
                            box.Item().Padding(4).Text(llegada).FontSize(8);
                        });
                    });

                    col.Item().PaddingTop(4);

                    // Section 4 — Parties
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Border(1).BorderColor(EvansBlue).Column(box =>
                        {
                            box.Item().Background(EvansBlue).Padding(3)
                                .Text("REMITENTE").FontColor(White).Bold().FontSize(7);
                            box.Item().Padding(4).Column(inner =>
                            {
                                inner.Item().Text(t =>
                                {
                                    t.Span("Ap. y Nombres / Razón Social: ").Bold().FontSize(7);
                                    t.Span(dto.RemitenteNombre).FontSize(8);
                                });
                                inner.Item().Text("R.U.C.:").FontSize(7);
                                inner.Item().Text("Tipo y Nº de Doc. Ident.:").FontSize(7);
                            });
                        });

                        row.ConstantItem(4);

                        row.RelativeItem().Border(1).BorderColor(EvansBlue).Column(box =>
                        {
                            box.Item().Background(EvansBlue).Padding(3)
                                .Text("DESTINATARIO").FontColor(White).Bold().FontSize(7);
                            box.Item().Padding(4).Column(inner =>
                            {
                                inner.Item().Text(t =>
                                {
                                    t.Span("Ap. y Nombres / Razón Social: ").Bold().FontSize(7);
                                    t.Span(dto.DestinatarioNombre).FontSize(8);
                                });
                                inner.Item().Text("R.U.C.:").FontSize(7);
                                inner.Item().Text("Tipo y Nº de Doc. Ident.:").FontSize(7);
                            });
                        });
                    });

                    col.Item().PaddingTop(4);

                    // Section 5 — Cargo table
                    col.Item().Border(1).BorderColor(EvansBlue).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(45);   // CANTIDAD
                            cols.RelativeColumn();     // DESCRIPCION
                            cols.ConstantColumn(45);   // PESO
                            cols.ConstantColumn(65);   // Unidad
                            cols.ConstantColumn(75);   // Costo Mínimo
                            cols.ConstantColumn(75);   // Valor Patrimonial
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(EvansBlue).Padding(3)
                                .Text("CANTIDAD").FontColor(White).Bold().FontSize(7).AlignCenter();
                            header.Cell().Background(EvansBlue).Padding(3)
                                .Text("DESCRIPCION").FontColor(White).Bold().FontSize(7).AlignCenter();
                            header.Cell().Background(EvansBlue).Padding(3)
                                .Text("PESO").FontColor(White).Bold().FontSize(7).AlignCenter();
                            header.Cell().Background(EvansBlue).Padding(3)
                                .Text("Unidad de Medida").FontColor(White).Bold().FontSize(7).AlignCenter();
                            header.Cell().Background(EvansBlue).Padding(3)
                                .Text("Costo Mínimo del traslado").FontColor(White).Bold().FontSize(7).AlignCenter();
                            header.Cell().Background(EvansBlue).Padding(3)
                                .Text("Valor Patrimonial").FontColor(White).Bold().FontSize(7).AlignCenter();
                        });

                        foreach (var d in dto.Detalles)
                        {
                            table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                .Text(d.Cantidad.ToString()).AlignCenter();
                            table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                .Text(d.Descripcion);
                            table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                .Text($"{d.PesoValor:F2}").AlignRight();
                            table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                .Text("KG").AlignCenter();
                            table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                .Text($"S/. {d.PrecioUnitario:F2}").AlignRight();
                            table.Cell().BorderBottom(1).BorderColor(LightGray).Padding(3)
                                .Text($"S/. {d.PrecioTotal:F2}").AlignRight();
                        }

                        // Empty row if no detalles (keeps table from collapsing)
                        if (!dto.Detalles.Any())
                        {
                            table.Cell().ColumnSpan(6).Padding(10).Text(" ");
                        }
                    });

                    col.Item().PaddingTop(4);

                    // Section 6 — Distancia / Generador
                    col.Item().Border(1).BorderColor(LightGray).Row(row =>
                    {
                        row.RelativeItem(2).Padding(4).Text(t =>
                        {
                            t.Span("DISTANCIA VIRTUAL:").Bold();
                        });
                        row.RelativeItem(1).BorderLeft(1).BorderColor(LightGray).Padding(4)
                            .Text("POR EL GENERADOR DE CARGA").AlignCenter();
                    });

                    col.Item().PaddingTop(4);

                    // Section 7 — Vehicle data
                    col.Item().Border(1).BorderColor(LightGray).Row(row =>
                    {
                        // LEFT: vehicle data
                        row.RelativeItem(2).Padding(4).Column(left =>
                        {
                            left.Item().Text("DATOS DEL VEHICULO Y DEL CONDUCTOR").Bold().FontSize(7);
                            left.Item().PaddingTop(3).Text("Marca del Vehículo:").FontSize(7);
                            left.Item().Text("Placa Nº:").FontSize(7);
                            left.Item().Text("Configuración Vehicular:").FontSize(7);
                            left.Item().Text("Nº Cert. de Inscripción:").FontSize(7);
                            left.Item().Text("Nº Licencia de Conducir:").FontSize(7);
                        });

                        // MIDDLE: subcontracted company
                        row.RelativeItem(2).BorderLeft(1).BorderColor(LightGray).Padding(4).Column(mid =>
                        {
                            mid.Item().Text("DATOS DE LA EMPRESA SUB-CONTRATADA").Bold().FontSize(7);
                            mid.Item().PaddingTop(3).Text("Nombres y Apellidos / Razón Social:").FontSize(7);
                            mid.Item().Text("Dirección:").FontSize(7);
                            mid.Item().Text("R.U.C.:").FontSize(7);
                        });

                        // RIGHT: signature
                        row.RelativeItem(1).BorderLeft(1).BorderColor(LightGray).Padding(4).Column(sig =>
                        {
                            sig.Item().Text("p. EVANS CARGO S.A.C.").AlignCenter().FontSize(7).Bold();
                            sig.Item().PaddingTop(20).LineHorizontal(1).LineColor(LightGray);
                            sig.Item().PaddingTop(4).Text("Conformidad del Cliente").AlignCenter().FontSize(7);
                        });
                    });

                    col.Item().PaddingTop(4);

                    // Section 8 — Observaciones
                    col.Item().Border(1).BorderColor(LightGray).MinHeight(40).Padding(4).Column(obs =>
                    {
                        obs.Item().Text("OBSERVACIONES:").Bold().FontSize(7);
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

    private static string FormatGuiaNumber(string numeroGuia)
    {
        // Input: "GREM-000001" → Output: "002 - 000001" style or use the number part
        // Keep as-is but reformat: split on '-'
        var parts = numeroGuia.Split('-');
        if (parts.Length == 2)
            return $"{parts[0]} - {parts[1]}";
        return numeroGuia;
    }

    private static byte[] LoadLogo()
    {
        var assembly = typeof(GuiaPdfRenderer).Assembly;
        const string name = "EVANS.Reports.Assets.logo.png";
        using var stream = assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException($"Embedded resource '{name}' not found.");
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
