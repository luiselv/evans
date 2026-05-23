using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Tests.Recepcion;

/// <summary>
/// Test-only connection factories that use fixed connection strings.
/// Used by all Recepcion integration tests.
/// </summary>
internal sealed class FixedConnectionFactory : IYearlyTransactionalConnectionFactory
{
    private readonly string _connectionString;

    public FixedConnectionFactory(string connectionString)
        => _connectionString = connectionString;

    public SqlConnection Create(int year) => new SqlConnection(_connectionString);
    public SqlConnection CreateForCurrentYear() => new SqlConnection(_connectionString);
}

internal sealed class FixedMasterConnectionFactory : IEvansMasterConnectionFactory
{
    private readonly string _connectionString;

    public FixedMasterConnectionFactory(string connectionString)
        => _connectionString = connectionString;

    public SqlConnection Create() => new SqlConnection(_connectionString);
}
