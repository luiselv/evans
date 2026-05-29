using Dapper;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Tests.Catalogo;

internal static class CatalogoSeed
{
    public static async Task EnsureClienteDireccionAsync(string connectionString, int clienteCodigo = 1)
    {
        await using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();

        await conn.ExecuteAsync(@"
            IF NOT EXISTS (SELECT 1 FROM DIRECCIONCLIENTE WHERE CLIE_CODIGO = @clienteCodigo)
                INSERT INTO DIRECCIONCLIENTE (CLIE_CODIGO, CLIE_DIRECCION, CLIE_CIUDAD, CLIE_PROVINCIA)
                VALUES (@clienteCodigo, 'Av Test 123', 'Lima', 'Lima');",
            new { clienteCodigo });
    }

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
