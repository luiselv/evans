using Dapper;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Tests.Catalogo;

internal static class CatalogoSeed
{
    public static async Task EnsureInactiveEstadoAsync(string connectionString)
    {
        await using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();

        await conn.ExecuteAsync(@"
            SET IDENTITY_INSERT ESTADO ON;
            IF NOT EXISTS (SELECT 1 FROM ESTADO WHERE ESTA_CODIGO = 2)
                INSERT INTO ESTADO (ESTA_CODIGO, ESTA_DESCRIPCION) VALUES (2, 'Inactivo');
            SET IDENTITY_INSERT ESTADO OFF;");
    }
}
