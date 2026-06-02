using EVANS.Application.Reportes.DTOs;

namespace EVANS.Application.Reportes.Ports;

public interface IReporteVentasExcelExporter
{
    byte[] Export(IReadOnlyList<VentaReporteDto> rows);
}
