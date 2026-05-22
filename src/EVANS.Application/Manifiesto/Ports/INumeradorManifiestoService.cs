using EVANS.Domain.Manifiesto;

namespace EVANS.Application.Manifiesto.Ports;

public interface INumeradorManifiestoService
{
    Task<NumeroManifiesto> IncrementarYObtenerAsync(int year, CancellationToken ct);
}
