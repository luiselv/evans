using Dapper;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Domain.Manifiesto;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Manifiesto;

/// <summary>
/// Increments PARAMETROS.PARA_MANIFIESTO on the master DB and returns the new NumeroManifiesto.
/// Uses its own connection (IEvansMasterConnectionFactory) — never shares the yearly-DB unit of work.
/// The UPDATE...OUTPUT pattern is atomic and safe for concurrent callers.
/// Counter is stored as nvarchar(15); cast to int for arithmetic, back to string for storage.
/// </summary>
public sealed class NumeradorManifiestoServiceSql(IEvansMasterConnectionFactory masterFactory)
    : INumeradorManifiestoService
{
    public async Task<NumeroManifiesto> IncrementarYObtenerAsync(int year, CancellationToken ct = default)
    {
        using var conn = masterFactory.Create();
        await conn.OpenAsync(ct);

        // Atomic increment: cast nvarchar→int, add 1, store back as nvarchar, return new value.
        // OUTPUT clause makes this a single statement — no ROWVERSION or explicit lock needed.
        var newCounterStr = await conn.ExecuteScalarAsync<string>(
            new CommandDefinition(
                @"UPDATE PARAMETROS
                  SET PARA_MANIFIESTO = CAST(CAST(PARA_MANIFIESTO AS INT) + 1 AS NVARCHAR(15))
                  OUTPUT inserted.PARA_MANIFIESTO",
                cancellationToken: ct));

        if (!int.TryParse(newCounterStr, out var counter))
            throw new InvalidOperationException(
                $"PARA_MANIFIESTO counter is not a valid integer: '{newCounterStr}'.");

        // Format: {YYYY}-{counter} — no zero-padding (spec I-7 regex: ^\d{{4}}-\d+$)
        return new NumeroManifiesto($"{year}-{counter}");
    }
}
