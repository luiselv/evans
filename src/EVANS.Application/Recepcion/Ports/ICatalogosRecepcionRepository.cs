using EVANS.Application.Recepcion.DTOs;

namespace EVANS.Application.Recepcion.Ports;

public interface ICatalogosRecepcionRepository
{
    Task<IReadOnlyList<ClienteLookupDto>> ListarClientesAsync(CancellationToken ct);
    Task<IReadOnlyList<DestinoLookupDto>> ListarDestinosAsync(CancellationToken ct);
    Task<IReadOnlyList<EstadoLookupDto>> ListarEstadosAsync(CancellationToken ct);
}
