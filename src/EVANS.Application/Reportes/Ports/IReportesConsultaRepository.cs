using EVANS.Application.Reportes.DTOs;

namespace EVANS.Application.Reportes.Ports;

public interface IReportesConsultaRepository
{
    Task<IReadOnlyList<EnvioMensualDto>> ConsultarEnviosMensualesAsync(
        EnviosMensualesFiltro filtro,
        int year,
        CancellationToken ct);
}
