using ClosedXML.Excel;
using EVANS.Application.Reportes.DTOs;
using EVANS.Infrastructure.External.Excel;

namespace EVANS.Infrastructure.Tests.Excel;

public sealed class ReporteVentasExcelExporterTests
{
    [Fact]
    public void Export_CreatesWorkbookWithRowsAndTotals()
    {
        var exporter = new ReporteVentasExcelExporter();
        var rows = new List<VentaReporteDto>
        {
            new(
                new DateTime(2024, 6, 10),
                1,
                "F001",
                "000001",
                "20111111111",
                "Cliente Uno",
                100m,
                18m,
                118m)
        };

        var bytes = exporter.Export(rows);

        using var stream = new MemoryStream(bytes);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet("Reporte ventas");

        worksheet.Cell(1, 1).GetString().Should().Be("Fecha");
        worksheet.Cell(2, 6).GetString().Should().Be("Cliente Uno");
        worksheet.Cell(2, 9).GetValue<decimal>().Should().Be(118m);
        worksheet.Cell(3, 6).GetString().Should().Be("TOTAL");
        worksheet.Cell(3, 9).FormulaA1.Should().Be("SUM(I2:I2)");
    }
}
