using Dapper;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.GuiaRemision;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.GuiaRemision;

/// <summary>
/// Updates Recepcion.RECE_GUIAREMISION to the guide's "SERIE-NUMERO" string.
/// Uses its own connection — never participates in the yearly-DB unit of work.
/// Must be called AFTER the UoW has committed (post-commit, separate transaction scope).
/// Idempotent: UPDATE with non-existent recepcionId is a no-op (0 rows affected), no exception.
/// </summary>
public sealed class RecepcionVinculadaServiceSql(
    IYearlyTransactionalConnectionFactory yearlyFactory) : IRecepcionVinculadaService
{
    public void VincularRecepcion(int recepcionId, NumeroGuia numero, int year)
    {
        using var conn = yearlyFactory.Create(year);
        conn.Open();

        conn.Execute(
            @"UPDATE Recepcion
              SET RECE_GUIAREMISION = @serieGuion
              WHERE RECE_CODIGO = @recepcionId",
            new
            {
                serieGuion = numero.ToString(),  // "SERIE-000042" format
                recepcionId
            });
    }
}
