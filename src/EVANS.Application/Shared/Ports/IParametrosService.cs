namespace EVANS.Application.Shared.Ports;

public interface IParametrosService
{
    Task<decimal> ObtenerIgvRateAsync(CancellationToken ct = default);
}
