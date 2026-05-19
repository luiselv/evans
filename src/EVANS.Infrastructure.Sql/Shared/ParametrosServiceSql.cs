using Dapper;
using EVANS.Application.Shared.Ports;
using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Extensions.Logging;

namespace EVANS.Infrastructure.Sql.Shared;

/// <summary>
/// SQL Server implementation of IParametrosService.
/// Reads PARA_IGV from the PARAMETROS table in the master (EVANS) database.
/// If the column is missing or returns NULL, falls back to the Peru statutory rate 0.18.
/// </summary>
public sealed class ParametrosServiceSql : IParametrosService
{
    private const decimal PeruStatutoryIgvRate = 0.18m;

    private readonly IEvansMasterConnectionFactory _masterFactory;
    private readonly ILogger<ParametrosServiceSql>? _logger;

    public ParametrosServiceSql(
        IEvansMasterConnectionFactory masterFactory,
        ILogger<ParametrosServiceSql>? logger = null)
    {
        _masterFactory = masterFactory;
        _logger        = logger;
    }

    public async Task<decimal> ObtenerIgvRateAsync(CancellationToken ct = default)
    {
        try
        {
            using var conn = _masterFactory.Create();
            await conn.OpenAsync(ct);

            var rate = await conn.ExecuteScalarAsync<double?>(
                new CommandDefinition("SELECT TOP 1 PARA_IGV FROM PARAMETROS", cancellationToken: ct));

            if (rate is null or 0.0)
            {
                _logger?.LogWarning(
                    "PARA_IGV returned NULL or 0 from PARAMETROS; " +
                    "falling back to statutory rate {Rate}.", PeruStatutoryIgvRate);
                return PeruStatutoryIgvRate;
            }

            return (decimal)rate;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex,
                "Failed to read PARA_IGV from PARAMETROS; " +
                "falling back to statutory rate {Rate}.", PeruStatutoryIgvRate);
            return PeruStatutoryIgvRate;
        }
    }
}
