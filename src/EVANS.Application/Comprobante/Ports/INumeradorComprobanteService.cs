using EVANS.Domain.Comprobante;

namespace EVANS.Application.Comprobante.Ports;

public interface INumeradorComprobanteService
{
    Task<NumeroComprobante> IncrementarYObtenerAsync(TipoComprobante tipo, CancellationToken ct = default);
}
