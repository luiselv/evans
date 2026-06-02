using ClosedXML.Excel;
using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;

namespace EVANS.Infrastructure.External.Excel;

public sealed class EnviosMensualesExcelExporter : IEnviosMensualesExcelExporter
{
    public byte[] Export(IReadOnlyList<EnvioMensualDto> rows)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Envios mensuales");

        worksheet.Cell(1, 1).Value = "Cliente";
        worksheet.Cell(1, 2).Value = "Nro. Guias";
        worksheet.Cell(1, 3).Value = "Ultimo Envio";
        worksheet.Range(1, 1, 1, 3).Style.Font.Bold = true;

        for (var i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            var excelRow = i + 2;

            worksheet.Cell(excelRow, 1).Value = row.Cliente;
            worksheet.Cell(excelRow, 2).Value = row.NroGuias;
            worksheet.Cell(excelRow, 3).Value = row.UltimoEnvio;
            worksheet.Cell(excelRow, 3).Style.DateFormat.Format = "dd/MM/yyyy";
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
