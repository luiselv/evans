using Dapper;
using EVANS.Application.Identidad.Ports;
using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Sql.Identidad;

public sealed class YearlyDatabaseProvisionerSql(
    IEvansMasterConnectionFactory masterFactory,
    IYearlyTransactionalConnectionFactory yearlyFactory,
    IYearlySchemaScriptProvider schemaScriptProvider) : IYearlyDatabaseProvisioner
{
    public YearlyDatabaseProvisionerSql(
        IEvansMasterConnectionFactory masterFactory,
        IYearlyTransactionalConnectionFactory yearlyFactory)
        : this(masterFactory, yearlyFactory, new PhysicalYearlySchemaScriptProvider())
    {
    }

    public async Task CreateYearAsync(int year, CancellationToken cancellationToken = default)
    {
        if (year is < 2000 or > 2999)
            throw new ArgumentOutOfRangeException(nameof(year), year, "Yearly database names must be numeric and start with 2.");

        var databaseName = year.ToString();
        var created = false;

        try
        {
            await using (var master = masterFactory.Create())
            {
                await master.OpenAsync(cancellationToken);
                if (await DatabaseExistsAsync(master, databaseName, cancellationToken))
                    throw new InvalidOperationException("Ya existe una Base de Datos para el año actual");

                await master.ExecuteAsync(new CommandDefinition(
                    $"CREATE DATABASE [{databaseName}]",
                    cancellationToken: cancellationToken));
                created = true;
            }

            var schema = await schemaScriptProvider.ReadSchemaAsync(cancellationToken);
            await using var yearly = yearlyFactory.Create(year);
            await yearly.OpenAsync(cancellationToken);
            foreach (var batch in SplitGoBatches(schema))
            {
                if (!string.IsNullOrWhiteSpace(batch))
                    await yearly.ExecuteAsync(new CommandDefinition(batch, cancellationToken: cancellationToken));
            }
        }
        catch
        {
            if (created)
                await DropDatabaseIfExistsAsync(databaseName, cancellationToken);
            throw;
        }
    }

    private static async Task<bool> DatabaseExistsAsync(
        SqlConnection connection,
        string databaseName,
        CancellationToken cancellationToken)
    {
        var count = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            "SELECT COUNT(*) FROM sys.databases WHERE name = @databaseName",
            new { databaseName },
            cancellationToken: cancellationToken));
        return count > 0;
    }

    private async Task DropDatabaseIfExistsAsync(string databaseName, CancellationToken cancellationToken)
    {
        await using var master = masterFactory.Create();
        await master.OpenAsync(cancellationToken);
        await master.ExecuteAsync(new CommandDefinition(
            $"IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @databaseName) BEGIN ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{databaseName}]; END",
            new { databaseName },
            cancellationToken: cancellationToken));
    }

    private static string[] SplitGoBatches(string sql) =>
        System.Text.RegularExpressions.Regex.Split(
            sql,
            @"^\s*GO\s*$",
            System.Text.RegularExpressions.RegexOptions.Multiline |
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
}
