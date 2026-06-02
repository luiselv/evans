using Dapper;
using EVANS.Application.Identidad.Ports;
using EVANS.Domain.Identidad;
using EVANS.Infrastructure.Sql.Catalogo;
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

    public async Task<CuentaUsuario?> GetByIdAsync(
        int codigo,
        CancellationToken cancellationToken = default)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(cancellationToken);

        var row = await conn.QuerySingleOrDefaultAsync<CuentaUsuarioRow>(
            new CommandDefinition(
                SelectCuentaSql + " WHERE USU_CODIGO = @codigo",
                new { codigo },
                cancellationToken: cancellationToken));

        return row is null ? null : MapCuenta(row);
    }

    public async Task<IReadOnlyList<CuentaUsuario>> SearchAsync(
        string? nombreCompletoPrefix,
        CancellationToken cancellationToken = default)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(cancellationToken);

        var sql = string.IsNullOrWhiteSpace(nombreCompletoPrefix)
            ? SelectCuentaSql + " ORDER BY USU_NOMBRECOMPLETO"
            : SelectCuentaSql + " WHERE USU_NOMBRECOMPLETO LIKE @prefix + '%' ORDER BY USU_NOMBRECOMPLETO";

        var rows = await conn.QueryAsync<CuentaUsuarioRow>(
            new CommandDefinition(
                sql,
                new { prefix = nombreCompletoPrefix?.Trim() },
                cancellationToken: cancellationToken));

        return rows.Select(MapCuenta).ToList().AsReadOnly();
    }

    public async Task<int> AddAsync(
        CuentaUsuario usuario,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO USUARIO (USU_NOMBREUSUARIO, USU_CLAVE, USU_NOMBRECOMPLETO, USU_ADMIN, ESTA_CODIGO)
            VALUES (@nombreUsuario, @clave, @nombreCompleto, @admin, @estadoCodigo);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(cancellationToken);

        return await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteScalarAsync<int>(new CommandDefinition(
                sql,
                ToParams(usuario),
                tx,
                cancellationToken: cancellationToken)), cancellationToken);
    }

    public async Task UpdateAsync(
        CuentaUsuario usuario,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE USUARIO
            SET USU_NOMBREUSUARIO = @nombreUsuario,
                USU_CLAVE = @clave,
                USU_NOMBRECOMPLETO = @nombreCompleto,
                USU_ADMIN = @admin,
                ESTA_CODIGO = @estadoCodigo
            WHERE USU_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(cancellationToken);

        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteAsync(new CommandDefinition(
                sql,
                ToParams(usuario),
                tx,
                cancellationToken: cancellationToken)), cancellationToken);
    }

    private const string SelectCuentaSql = @"
        SELECT
            USU_CODIGO AS Codigo,
            USU_NOMBREUSUARIO AS NombreUsuario,
            ISNULL(USU_CLAVE, '') AS Clave,
            ISNULL(USU_NOMBRECOMPLETO, '') AS NombreCompleto,
            ISNULL(USU_ADMIN, 0) AS Admin,
            ISNULL(ESTA_CODIGO, 0) AS EstadoCodigo
        FROM USUARIO";

    private static CuentaUsuario MapCuenta(CuentaUsuarioRow row) =>
        CuentaUsuario.Materializar(
            row.Codigo,
            row.NombreUsuario,
            row.Clave,
            row.NombreCompleto,
            row.Admin == 1,
            row.EstadoCodigo);

    private static object ToParams(CuentaUsuario usuario) => new
    {
        codigo = usuario.Codigo,
        nombreUsuario = usuario.NombreUsuario,
        clave = usuario.Clave,
        nombreCompleto = usuario.NombreCompleto,
        admin = usuario.EsAdministrador ? 1 : 0,
        estadoCodigo = usuario.EstadoCodigo
    };

    private sealed record UsuarioRow(
        string NombreUsuario,
        string NombreCompleto,
        int Admin);

    private sealed record CuentaUsuarioRow(
        int Codigo,
        string NombreUsuario,
        string Clave,
        string NombreCompleto,
        int Admin,
        int EstadoCodigo);
}
