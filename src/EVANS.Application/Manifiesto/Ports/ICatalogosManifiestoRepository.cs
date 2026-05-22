using EVANS.Application.Manifiesto.DTOs;

namespace EVANS.Application.Manifiesto.Ports;

public interface ICatalogosManifiestoRepository
{
    Task<CatalogosManifiestoDto> ObtenerCatalogosAsync(CancellationToken ct);
}
