using EVANS.Domain.Comprobante;

namespace EVANS.Application.Comprobante.Ports;

public interface IGuiaVinculadaService
{
    Task<bool> VincularComprobanteAsync(string guiaRef, NumeroComprobante numero, int año, CancellationToken ct = default);
}
