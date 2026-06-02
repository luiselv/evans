using EVANS.Application.Shared.DTOs;

namespace EVANS.Application.Shared.Ports;

public interface IParametrosService
{
    Task<ParametrosDto> ObtenerParametrosAsync(CancellationToken ct = default);
    Task<decimal> ObtenerIgvRateAsync(CancellationToken ct = default);
}
