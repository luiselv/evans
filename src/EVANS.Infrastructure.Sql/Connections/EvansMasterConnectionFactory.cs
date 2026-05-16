using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EVANS.Infrastructure.Sql.Connections;

internal sealed class EvansMasterConnectionFactory(IConfiguration configuration) : IEvansMasterConnectionFactory
{
    private readonly string _connectionString = configuration.GetConnectionString("EvansMaster")
        ?? throw new InvalidOperationException("ConnectionStrings:EvansMaster is required.");

    public SqlConnection Create() => new(_connectionString);
}
