using Dapper;
using EVANS.Application.Identidad.Ports;
using EVANS.Domain.Identidad;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Identidad;

public sealed class UsuarioRepositorySql : IUsuarioRepository
{
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public UsuarioRepositorySql(IEvansMasterConnectionFactory masterFactory)
        => _masterFactory = masterFactory;

    public async Task<Usuario?> AuthenticateAsync(
        string nombreUsuario,
        string clave,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT
                USU_NOMBREUSUARIO AS NombreUsuario,
                ISNULL(USU_NOMBRECOMPLETO, USU_NOMBREUSUARIO) AS NombreCompleto,
                ISNULL(USU_ADMIN, 0) AS Admin
            FROM USUARIO
            WHERE USU_NOMBREUSUARIO = @nombreUsuario
              AND USU_CLAVE = @clave
              AND ESTA_CODIGO = 1";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(cancellationToken);

        var row = await conn.QuerySingleOrDefaultAsync<UsuarioRow>(
            new CommandDefinition(
                sql,
                new { nombreUsuario, clave },
                cancellationToken: cancellationToken));

        return row is null
            ? null
            : Usuario.Autenticado(
                row.NombreUsuario,
                row.NombreCompleto,
                row.Admin == 1);
    }

    private sealed record UsuarioRow(
        string NombreUsuario,
        string NombreCompleto,
        int Admin);
}
