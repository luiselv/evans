using System.Data;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Sql.GuiaRemision;

/// <summary>
/// SQL Server implementation of IUnitOfWork for yearly transactional databases.
/// Opens a Serializable transaction on construction; commits on Commit(); rolls back on Dispose() if not committed.
/// Internal members (Connection, Transaction) are accessible only within the EVANS.Infrastructure.Sql assembly.
/// </summary>
internal sealed class SqlUnitOfWork : IUnitOfWork
{
    private bool _committed;
    private bool _disposed;

    internal SqlConnection Connection { get; }
    internal SqlTransaction Transaction { get; }

    public SqlUnitOfWork(IYearlyTransactionalConnectionFactory factory, int year)
    {
        Connection = factory.Create(year);
        Connection.Open();
        Transaction = Connection.BeginTransaction(IsolationLevel.Serializable);
    }

    public void Commit()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(SqlUnitOfWork));
        if (_committed) return;

        Transaction.Commit();
        _committed = true;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        try
        {
            if (!_committed)
            {
                try { Transaction.Rollback(); }
                catch { /* Rollback after connection loss — ignore */ }
            }
        }
        finally
        {
            Transaction.Dispose();
            Connection.Dispose();
        }
    }
}
