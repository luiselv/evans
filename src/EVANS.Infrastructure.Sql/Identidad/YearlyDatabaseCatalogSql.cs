using Dapper;
using EVANS.Application.Identidad.Ports;
using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Sql.Identidad;

public sealed class YearlyDatabaseCatalogSql(IEvansMasterConnectionFactory masterFactory) : IYearlyDatabaseCatalog
{
    public async Task<IReadOnlyList<int>> ListYearsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT TRY_CONVERT(int, name)
            FROM sys.databases
            WHERE name LIKE '2%'
              AND TRY_CONVERT(int, name) IS NOT NULL
            ORDER BY name";

        await using var conn = masterFactory.Create();
        await conn.OpenAsync(cancellationToken);

        var years = await conn.QueryAsync<int>(new CommandDefinition(sql, cancellationToken: cancellationToken));
        return years.ToList().AsReadOnly();
    }
}

