using ClosedXML.Excel;
using EVANS.Application.Reportes.DTOs;
using EVANS.Infrastructure.External.Excel;

namespace EVANS.Infrastructure.Tests.Excel;

public sealed class EnviosMensualesExcelExporterTests
{
    [Fact]
    public void Export_ReturnsXlsxWithHeadersAndRows()
    {
        var exporter = new EnviosMensualesExcelExporter();
        var rows = new List<EnvioMensualDto>
        {
            new("Cliente Uno", 3, new DateTime(2024, 6, 20)),
            new("Cliente Dos", 1, new DateTime(2024, 6, 15))
        };

        var bytes = exporter.Export(rows);

        bytes.Should().NotBeNullOrEmpty();
        bytes[0].Should().Be((byte)'P');
        bytes[1].Should().Be((byte)'K');

        using var stream = new MemoryStream(bytes);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet("Envios mensuales");

        worksheet.Cell(1, 1).GetString().Should().Be("Cliente");
        worksheet.Cell(1, 2).GetString().Should().Be("Nro. Guias");
        worksheet.Cell(1, 3).GetString().Should().Be("Ultimo Envio");

        worksheet.Cell(2, 1).GetString().Should().Be("Cliente Uno");
        worksheet.Cell(2, 2).GetValue<int>().Should().Be(3);
        worksheet.Cell(2, 3).GetDateTime().Should().Be(new DateTime(2024, 6, 20));

        worksheet.Cell(3, 1).GetString().Should().Be("Cliente Dos");
        worksheet.Cell(3, 2).GetValue<int>().Should().Be(1);
        worksheet.Cell(3, 3).GetDateTime().Should().Be(new DateTime(2024, 6, 15));
    }
}
