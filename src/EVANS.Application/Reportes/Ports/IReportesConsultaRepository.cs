using EVANS.Application.Reportes.DTOs;

namespace EVANS.Application.Reportes.Ports;

public interface IReportesConsultaRepository
{
    Task<IReadOnlyList<ClienteReporteDto>> ListarClientesAsync(CancellationToken ct);

    Task<IReadOnlyList<DestinoReporteDto>> ListarDestinosActivosAsync(CancellationToken ct);

    Task<IReadOnlyList<EnvioMensualDto>> ConsultarEnviosMensualesAsync(
        EnviosMensualesFiltro filtro,
        int year,
        CancellationToken ct);

    Task<IReadOnlyList<GuiaPorClienteDto>> ConsultarGuiasPorClienteAsync(
        GuiasPorClienteFiltro filtro,
        int year,
        CancellationToken ct);
}
