using Dapper;
using EVANS.Application.Comprobante.Ports;
using EVANS.Domain.Comprobante;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Comprobante;

/// <summary>
/// Increments PARAMETROS.PARA_FACTNRO1 or PARA_BOLNRO1 on the master DB and returns
/// the new NumeroComprobante. Uses its own connection — never shares the yearly-DB unit of work.
/// Race condition gaps in the sequence are accepted per spec (same as NumeradorServiceSql for Guia).
/// </summary>
public sealed class NumeradorComprobanteServiceSql(IEvansMasterConnectionFactory masterFactory)
    : INumeradorComprobanteService
{
    public async Task<NumeroComprobante> IncrementarYObtenerAsync(TipoComprobante tipo, CancellationToken ct = default)
    {
        using var conn = masterFactory.Create();
        await conn.OpenAsync(ct);

        var row = tipo switch
        {
            TipoComprobante.Factura => await conn.QuerySingleAsync(
                new CommandDefinition(
                    @"UPDATE PARAMETROS SET PARA_FACTNRO1 =
                          RIGHT('000000' + CAST(CAST(PARA_FACTNRO1 AS INT) + 1 AS VARCHAR(6)), 6)
                      OUTPUT inserted.PARA_FACTSERIE, inserted.PARA_FACTNRO1",
                    cancellationToken: ct)),

            TipoComprobante.Boleta => await conn.QuerySingleAsync(
                new CommandDefinition(
                    @"UPDATE PARAMETROS SET PARA_BOLNRO1 =
                          RIGHT('000000' + CAST(CAST(PARA_BOLNRO1 AS INT) + 1 AS VARCHAR(6)), 6)
                      OUTPUT inserted.PARA_BOLSERIE, inserted.PARA_BOLNRO1",
                    cancellationToken: ct)),

            _ => throw new InvalidOperationException($"Unknown TipoComprobante: {tipo}")
        };

        string serie;
        string numeroStr;

        if (tipo == TipoComprobante.Factura)
        {
            serie = row.PARA_FACTSERIE;
            numeroStr = row.PARA_FACTNRO1;
        }
        else
        {
            serie = row.PARA_BOLSERIE;
            numeroStr = row.PARA_BOLNRO1;
        }

        if (!int.TryParse(numeroStr, out var _))
            throw new InvalidOperationException(
                $"Counter value is not a valid integer: '{numeroStr}'.");

        if (string.IsNullOrWhiteSpace(serie) || serie.Length != 4)
            throw new InvalidOperationException(
                $"Serie is not a valid 4-character serie: '{serie}'.");

        return new NumeroComprobante(serie, numeroStr);
    }
}
