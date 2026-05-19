using Dapper;
using EVANS.Application.Comprobante.Ports;
using EVANS.Domain.Comprobante;
using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Extensions.Logging;

namespace EVANS.Infrastructure.Sql.Comprobante;

/// <summary>
/// Updates GuiaRemision.GREM_DOCVENTA to the comprobante number string (SERIE-NUMERO).
/// Uses its own connection — never participates in the yearly-DB unit of work.
/// Must be called AFTER the UoW has committed (post-commit, best-effort).
/// Returns false on 0 rows affected or any exception; never rethrows.
/// </summary>
public sealed class GuiaVinculadaServiceSql(
    IYearlyTransactionalConnectionFactory yearlyFactory,
    ILogger<GuiaVinculadaServiceSql> logger) : IGuiaVinculadaService
{
    public async Task<bool> VincularComprobanteAsync(string guiaRef, NumeroComprobante numero, int año, CancellationToken ct = default)
    {
        try
        {
            using var conn = yearlyFactory.Create(año);
            await conn.OpenAsync(ct);

            // COMP_GRT stores concatenated "SERIENUMERO" (no dash), e.g. "F001000001".
            // GREM_SERIE + GREM_NUMERO concatenation matches the guiaRef format.
            // GREM_DOCVENTA is updated to the new "SERIE-NUMERO" display format.
            var rowsAffected = await conn.ExecuteAsync(
                new CommandDefinition(
                    @"UPDATE GuiaRemision
                      SET GREM_DOCVENTA = @comprobanteNro
                      WHERE GREM_SERIE + GREM_NUMERO = @guiaRef",
                    new
                    {
                        comprobanteNro = numero.ToString(),  // "F001-000001" format
                        guiaRef
                    },
                    cancellationToken: ct));

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "VincularComprobante failed for GuiaRef={GuiaRef}", guiaRef);
            return false;
        }
    }
}
