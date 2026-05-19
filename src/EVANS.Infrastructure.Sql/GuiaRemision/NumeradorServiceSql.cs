using Dapper;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.GuiaRemision;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.GuiaRemision;

/// <summary>
/// Increments PARAMETROS.PARA_GREMNRO1 on the master DB and returns the new NumeroGuia.
/// Uses its own connection — never shares the yearly-DB unit of work.
/// Race condition gaps in the sequence are accepted per spec (INumeradorService-RaceCondition).
/// </summary>
public sealed class NumeradorServiceSql(IEvansMasterConnectionFactory masterFactory) : INumeradorService
{
    public NumeroGuia IncrementarYObtenerGuia()
    {
        using var conn = masterFactory.Create();
        conn.Open();

        // Atomic increment + read in a single round-trip
        var row = conn.QuerySingle(
            @"UPDATE PARAMETROS SET PARA_GREMNRO1 =
                  RIGHT('000000' + CAST(CAST(PARA_GREMNRO1 AS INT) + 1 AS VARCHAR(6)), 6)
              OUTPUT inserted.PARA_GREMSERIE, inserted.PARA_GREMNRO1");

        string serie = row.PARA_GREMSERIE;
        string numeroStr = row.PARA_GREMNRO1;

        if (!int.TryParse(numeroStr, out var numero))
            throw new InvalidOperationException(
                $"PARA_GREMNRO1 is not a valid integer: '{numeroStr}'.");

        if (string.IsNullOrWhiteSpace(serie) || serie.Length != 4)
            throw new InvalidOperationException(
                $"PARA_GREMSERIE is not a valid 4-character serie: '{serie}'.");

        return new NumeroGuia(serie, numero);
    }
}
