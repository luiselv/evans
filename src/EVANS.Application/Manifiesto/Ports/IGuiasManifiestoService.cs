using EVANS.Domain.Manifiesto;

namespace EVANS.Application.Manifiesto.Ports;

public interface IGuiasManifiestoService
{
    Task<GuiasMarcadoResult> MarcarGuiasEnviadasAsync(
        IReadOnlyList<int> guiaIds,
        string numero,
        DateTime fechaTraslado,
        CarrierInfo carrier,
        int year,
        CancellationToken ct);

    Task<GuiasMarcadoResult> MarcarGuiasDisponiblesAsync(
        IReadOnlyList<int> guiaIds,
        int year,
        CancellationToken ct);
}
