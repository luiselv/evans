namespace EVANS.Infrastructure.Sql.Identidad;

public interface IYearlySchemaScriptProvider
{
    Task<string> ReadSchemaAsync(CancellationToken cancellationToken = default);
}
