using System.Data;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Sql.Catalogo;

internal static class CatalogoSqlWriteScope
{
    public static async Task ExecuteAsync(
        SqlConnection conn,
        Func<SqlTransaction, Task> action,
        CancellationToken ct)
    {
        await using var tx = (SqlTransaction)await conn.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        await action(tx);
        await tx.CommitAsync(ct);
    }

    public static async Task<T> ExecuteAsync<T>(
        SqlConnection conn,
        Func<SqlTransaction, Task<T>> action,
        CancellationToken ct)
    {
        await using var tx = (SqlTransaction)await conn.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        var result = await action(tx);
        await tx.CommitAsync(ct);
        return result;
    }
}
