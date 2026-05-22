using Dapper;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Domain.Manifiesto;
using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Extensions.Logging;

namespace EVANS.Infrastructure.Sql.Manifiesto;

/// <summary>
/// Updates GuiaRemision rows as part of manifiesto operations.
/// Uses its own yearly-DB connection — never shares the UoW transaction.
/// Both methods must be called AFTER the UoW has committed (post-commit, best-effort).
/// Returns GuiasMarcadoResult — never throws.
/// </summary>
public sealed class GuiasManifiestoServiceSql(
    IYearlyTransactionalConnectionFactory yearlyFactory,
    ILogger<GuiasManifiestoServiceSql> logger) : IGuiasManifiestoService
{
    public async Task<GuiasMarcadoResult> MarcarGuiasEnviadasAsync(
        IReadOnlyList<int> guiaIds,
        string numero,
        DateTime fechaTraslado,
        CarrierInfo carrier,
        int year,
        CancellationToken ct = default)
    {
        if (guiaIds.Count == 0)
            return new GuiasMarcadoResult(0, []);

        try
        {
            using var conn = yearlyFactory.Create(year);
            await conn.OpenAsync(ct);

            // CRITICAL: CARR_CODIGO must be DBNull.Value (not 0) when null — spec CRITICAL note #1
            var rowsAffected = await conn.ExecuteAsync(
                new CommandDefinition(
                    @"UPDATE GuiaRemision
                      SET GREM_FECHATRASLADO = @fechaTraslado,
                          GREM_ENVIADO       = 1,
                          EMPR_CODIGO        = @transportistaCodigo,
                          CHOF_CODIGO        = @choferCodigo,
                          VEHI_CODIGO        = @vehiculoCodigo,
                          CARR_CODIGO        = @carretaCodigo,
                          GREM_MANIFIESTO    = 1,
                          GREM_NROMANIFIESTO = @numero
                      WHERE GREM_CODIGO IN @ids",
                    new
                    {
                        fechaTraslado,
                        transportistaCodigo = carrier.TransportistaCodigo,
                        choferCodigo = carrier.ChoferCodigo,
                        vehiculoCodigo = carrier.VehiculoCodigo,
                        carretaCodigo = (object?)carrier.CarretaCodigo ?? DBNull.Value,
                        numero,
                        ids = guiaIds
                    },
                    cancellationToken: ct));

            // Determine which IDs were not found
            var notFound = new List<int>();
            if (rowsAffected < guiaIds.Count)
            {
                var found = (await conn.QueryAsync<int>(
                    new CommandDefinition(
                        "SELECT GREM_CODIGO FROM GuiaRemision WHERE GREM_CODIGO IN @ids",
                        new { ids = guiaIds },
                        cancellationToken: ct))).ToHashSet();

                notFound.AddRange(guiaIds.Where(id => !found.Contains(id)));
            }

            return new GuiasMarcadoResult(rowsAffected, notFound);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "MarcarGuiasEnviadasAsync failed for {Count} guias", guiaIds.Count);
            return new GuiasMarcadoResult(0, guiaIds.ToList());
        }
    }

    public async Task<GuiasMarcadoResult> MarcarGuiasDisponiblesAsync(
        IReadOnlyList<int> guiaIds,
        int year,
        CancellationToken ct = default)
    {
        if (guiaIds.Count == 0)
            return new GuiasMarcadoResult(0, []);

        try
        {
            using var conn = yearlyFactory.Create(year);
            await conn.OpenAsync(ct);

            // SC-4: GREM_MANIFIESTO is NOT reset on disponibles/eliminar path (legacy behavior preserved).
            // Only GREM_ENVIADO, GREM_NROMANIFIESTO, and GREM_FECHATRASLADO are reset.
            var rowsAffected = await conn.ExecuteAsync(
                new CommandDefinition(
                    @"UPDATE GuiaRemision
                      SET GREM_ENVIADO       = 0,
                          GREM_NROMANIFIESTO = '',
                          GREM_FECHATRASLADO = GREM_FECHAEMISION
                      WHERE GREM_CODIGO IN @ids",
                    new { ids = guiaIds },
                    cancellationToken: ct));

            var notFound = new List<int>();
            if (rowsAffected < guiaIds.Count)
            {
                var found = (await conn.QueryAsync<int>(
                    new CommandDefinition(
                        "SELECT GREM_CODIGO FROM GuiaRemision WHERE GREM_CODIGO IN @ids",
                        new { ids = guiaIds },
                        cancellationToken: ct))).ToHashSet();

                notFound.AddRange(guiaIds.Where(id => !found.Contains(id)));
            }

            return new GuiasMarcadoResult(rowsAffected, notFound);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "MarcarGuiasDisponiblesAsync failed for {Count} guias", guiaIds.Count);
            return new GuiasMarcadoResult(0, guiaIds.ToList());
        }
    }
}
