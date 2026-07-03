using Dapper;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Catalogo;

public sealed class ClienteRepositorySql : IClienteRepository
{
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public ClienteRepositorySql(IEvansMasterConnectionFactory masterFactory)
        => _masterFactory = masterFactory;

    public async Task<Cliente?> GetByIdAsync(int codigo, CancellationToken ct)
    {
        const string sql = @"
            SELECT
                CLIE_CODIGO AS Codigo,
                ISNULL(CLIE_NOMBRE, '') AS RazonSocial,
                ISNULL(IDEN_CODIGO, 0) AS TipoIdCodigo,
                ISNULL(CLIE_NROIDENTIFICACION, '') AS NroIdentificacion,
                CLIE_TELEFONO AS Telefono,
                CLIE_FAX AS Fax,
                CLIE_EMAIL AS Email,
                CLIE_REPRESENTANTE AS Representante
            FROM CLIENTE
            WHERE CLIE_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var row = await conn.QueryFirstOrDefaultAsync<ClienteRow>(
            new CommandDefinition(sql, new { codigo }, cancellationToken: ct));

        return row is null ? null : Map(row, await ListDireccionesAsync(conn, row.Codigo, ct));
    }

    public async Task<IReadOnlyList<Cliente>> ListAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT
                CLIE_CODIGO AS Codigo,
                ISNULL(CLIE_NOMBRE, '') AS RazonSocial,
                ISNULL(IDEN_CODIGO, 0) AS TipoIdCodigo,
                ISNULL(CLIE_NROIDENTIFICACION, '') AS NroIdentificacion,
                CLIE_TELEFONO AS Telefono,
                CLIE_FAX AS Fax,
                CLIE_EMAIL AS Email,
                CLIE_REPRESENTANTE AS Representante
            FROM CLIENTE
            ORDER BY CLIE_NOMBRE";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var rows = (await conn.QueryAsync<ClienteRow>(
            new CommandDefinition(sql, cancellationToken: ct))).ToList();

        var clientes = new List<Cliente>(rows.Count);
        foreach (var row in rows)
            clientes.Add(Map(row, await ListDireccionesAsync(conn, row.Codigo, ct)));

        return clientes.AsReadOnly();
    }

    public async Task<int> AddAsync(Cliente cliente, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO CLIENTE (CLIE_NOMBRE, IDEN_CODIGO, CLIE_NROIDENTIFICACION, CLIE_TELEFONO, CLIE_FAX, CLIE_EMAIL, CLIE_REPRESENTANTE)
            VALUES (@razonSocial, @tipoIdCodigo, @nroIdentificacion, @telefono, @fax, @email, @representante);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        return await CatalogoSqlWriteScope.ExecuteAsync(conn, async tx =>
        {
            var codigo = await conn.ExecuteScalarAsync<int>(
                new CommandDefinition(sql, ToParameters(cliente), tx, cancellationToken: ct));

            await ReplaceDireccionesAsync(conn, tx, codigo, cliente.Direcciones, ct);
            return codigo;
        }, ct);
    }

    public async Task UpdateAsync(Cliente cliente, CancellationToken ct)
    {
        const string sql = @"
            UPDATE CLIENTE
            SET CLIE_NOMBRE = @razonSocial,
                IDEN_CODIGO = @tipoIdCodigo,
                CLIE_NROIDENTIFICACION = @nroIdentificacion,
                CLIE_TELEFONO = @telefono,
                CLIE_FAX = @fax,
                CLIE_EMAIL = @email,
                CLIE_REPRESENTANTE = @representante
            WHERE CLIE_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        await CatalogoSqlWriteScope.ExecuteAsync(conn, async tx =>
        {
            await conn.ExecuteAsync(new CommandDefinition(sql, ToParameters(cliente), tx, cancellationToken: ct));
            await ReplaceDireccionesAsync(conn, tx, cliente.Codigo, cliente.Direcciones, ct);
        }, ct);
    }

    private static async Task<IReadOnlyList<Direccion>> ListDireccionesAsync(
        Microsoft.Data.SqlClient.SqlConnection conn,
        int clienteCodigo,
        CancellationToken ct)
    {
        const string sql = @"
            SELECT
                ISNULL(CLIE_DIRECCION, '') AS Calle,
                ISNULL(CLIE_CIUDAD, '') AS Ciudad,
                ISNULL(CLIE_PROVINCIA, '') AS Provincia
            FROM DIRECCIONCLIENTE
            WHERE CLIE_CODIGO = @clienteCodigo";

        var direcciones = (await conn.QueryAsync<Direccion>(
            new CommandDefinition(sql, new { clienteCodigo }, cancellationToken: ct))).ToList();

        return direcciones.AsReadOnly();
    }

    private static async Task ReplaceDireccionesAsync(
        Microsoft.Data.SqlClient.SqlConnection conn,
        Microsoft.Data.SqlClient.SqlTransaction tx,
        int clienteCodigo,
        IReadOnlyList<Direccion> direcciones,
        CancellationToken ct)
    {
        await conn.ExecuteAsync(new CommandDefinition(
            "DELETE FROM DIRECCIONCLIENTE WHERE CLIE_CODIGO = @clienteCodigo",
            new { clienteCodigo },
            tx,
            cancellationToken: ct));

        const string insertSql = @"
            INSERT INTO DIRECCIONCLIENTE (CLIE_CODIGO, CLIE_DIRECCION, CLIE_CIUDAD, CLIE_PROVINCIA)
            VALUES (@clienteCodigo, @calle, @ciudad, @provincia)";

        foreach (var direccion in direcciones)
        {
            await conn.ExecuteAsync(new CommandDefinition(
                insertSql,
                new
                {
                    clienteCodigo,
                    calle = direccion.Calle,
                    ciudad = direccion.Ciudad,
                    provincia = direccion.Provincia
                },
                tx,
                cancellationToken: ct));
        }
    }

    private static Cliente Map(ClienteRow row, IReadOnlyList<Direccion> direcciones) =>
        Cliente.Materializar(
            row.Codigo,
            row.RazonSocial,
            row.TipoIdCodigo,
            row.NroIdentificacion,
            row.Telefono,
            row.Fax,
            row.Email,
            row.Representante,
            direcciones);

    private static object ToParameters(Cliente cliente) => new
    {
        codigo = cliente.Codigo,
        razonSocial = cliente.RazonSocial,
        tipoIdCodigo = cliente.TipoIdCodigo,
        nroIdentificacion = cliente.NroIdentificacion,
        telefono = cliente.Telefono,
        fax = cliente.Fax,
        email = cliente.Email,
        representante = cliente.Representante
    };

    private sealed record ClienteRow(
        int Codigo,
        string RazonSocial,
        int TipoIdCodigo,
        string NroIdentificacion,
        string? Telefono,
        string? Fax,
        string? Email,
        string? Representante);
}
