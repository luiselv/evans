using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Tests.Catalogo;

internal sealed class FixedMasterConnectionFactory : IEvansMasterConnectionFactory
{
    private readonly string _connectionString;

    public FixedMasterConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection Create() => new(_connectionString);
}
