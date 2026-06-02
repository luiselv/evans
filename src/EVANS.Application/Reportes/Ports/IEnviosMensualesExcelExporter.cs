using EVANS.Application.Reportes.DTOs;

namespace EVANS.Application.Reportes.Ports;

public interface IEnviosMensualesExcelExporter
{
    byte[] Export(IReadOnlyList<EnvioMensualDto> rows);
}
