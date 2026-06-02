using ClosedXML.Excel;
using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;

namespace EVANS.Infrastructure.External.Excel;

public sealed class ReporteVentasExcelExporter : IReporteVentasExcelExporter
{
    public byte[] Export(IReadOnlyList<VentaReporteDto> rows)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Reporte ventas");

        worksheet.Cell(1, 1).Value = "Fecha";
        worksheet.Cell(1, 2).Value = "Tipo";
        worksheet.Cell(1, 3).Value = "Serie";
        worksheet.Cell(1, 4).Value = "Número";
        worksheet.Cell(1, 5).Value = "Nro ID";
        worksheet.Cell(1, 6).Value = "Cliente";
        worksheet.Cell(1, 7).Value = "Valor Venta";
        worksheet.Cell(1, 8).Value = "IGV";
        worksheet.Cell(1, 9).Value = "Total";

        for (var i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            var excelRow = i + 2;
            worksheet.Cell(excelRow, 1).Value = row.Fecha;
            worksheet.Cell(excelRow, 2).Value = row.TipoCodigo;
            worksheet.Cell(excelRow, 3).Value = row.Serie;
            worksheet.Cell(excelRow, 4).Value = row.Numero;
            worksheet.Cell(excelRow, 5).Value = row.ClienteNumeroIdentificacion;
            worksheet.Cell(excelRow, 6).Value = row.ClienteNombre;
            worksheet.Cell(excelRow, 7).Value = row.ValorVenta;
            worksheet.Cell(excelRow, 8).Value = row.Igv;
            worksheet.Cell(excelRow, 9).Value = row.Total;
        }

        var totalRow = rows.Count + 2;
        worksheet.Cell(totalRow, 6).Value = "TOTAL";
        worksheet.Cell(totalRow, 7).FormulaA1 = $"SUM(G2:G{totalRow - 1})";
        worksheet.Cell(totalRow, 8).FormulaA1 = $"SUM(H2:H{totalRow - 1})";
        worksheet.Cell(totalRow, 9).FormulaA1 = $"SUM(I2:I{totalRow - 1})";

        worksheet.Range(1, 1, 1, 9).Style.Font.Bold = true;
        worksheet.Range(totalRow, 6, totalRow, 9).Style.Font.Bold = true;
        worksheet.Column(1).Style.DateFormat.Format = "dd/MM/yyyy";
        worksheet.Columns(7, 9).Style.NumberFormat.Format = "#,##0.00";
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
