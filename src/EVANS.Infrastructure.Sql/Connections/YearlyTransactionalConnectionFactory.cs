using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EVANS.Infrastructure.Sql.Connections;

internal sealed class YearlyTransactionalConnectionFactory(IConfiguration configuration) : IYearlyTransactionalConnectionFactory
{
    private readonly string _template = configuration.GetConnectionString("EvansYearly")
        ?? throw new InvalidOperationException("ConnectionStrings:EvansYearly is required.");

    public SqlConnection Create(int year)
    {
        // Template uses {year} placeholder: "Server=...;Database={year};..."
        var connStr = _template.Replace("{year}", year.ToString(), StringComparison.OrdinalIgnoreCase);
        return new SqlConnection(connStr);
    }

    public SqlConnection CreateForCurrentYear() => Create(DateTime.Today.Year);
}
