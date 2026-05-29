using Dapper;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Catalogo;

public sealed class TipoIdentificacionRepositorySql : ITipoIdentificacionRepository
{
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public TipoIdentificacionRepositorySql(IEvansMasterConnectionFactory masterFactory)
        => _masterFactory = masterFactory;

    public async Task<TipoIdentificacion?> GetByIdAsync(int codigo, CancellationToken ct)
    {
        const string sql = @"
            SELECT IDEN_CODIGO AS Codigo, ISNULL(IDEN_DESCRIPCION, '') AS Descripcion
            FROM TIPOIDENTIFICACION
            WHERE IDEN_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var row = await conn.QueryFirstOrDefaultAsync<TipoIdentificacionRow>(
            new CommandDefinition(sql, new { codigo }, cancellationToken: ct));

        return row is null ? null : TipoIdentificacion.Materializar(row.Codigo, row.Descripcion);
    }

    public async Task<IReadOnlyList<TipoIdentificacion>> ListAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT IDEN_CODIGO AS Codigo, ISNULL(IDEN_DESCRIPCION, '') AS Descripcion
            FROM TIPOIDENTIFICACION
            ORDER BY IDEN_DESCRIPCION";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<TipoIdentificacionRow>(
            new CommandDefinition(sql, cancellationToken: ct));

        return rows
            .Select(row => TipoIdentificacion.Materializar(row.Codigo, row.Descripcion))
            .ToList()
            .AsReadOnly();
    }

    public async Task<int> AddAsync(TipoIdentificacion tipoIdentificacion, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO TIPOIDENTIFICACION (IDEN_DESCRIPCION)
            VALUES (@descripcion);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        return await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteScalarAsync<int>(
                new CommandDefinition(sql, new { descripcion = tipoIdentificacion.Descripcion }, tx, cancellationToken: ct)), ct);
    }

    public async Task UpdateAsync(TipoIdentificacion tipoIdentificacion, CancellationToken ct)
    {
        const string sql = @"
            UPDATE TIPOIDENTIFICACION
            SET IDEN_DESCRIPCION = @descripcion
            WHERE IDEN_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteAsync(new CommandDefinition(
                sql,
                new { codigo = tipoIdentificacion.Codigo, descripcion = tipoIdentificacion.Descripcion },
                tx,
                cancellationToken: ct)), ct);
    }

    private sealed record TipoIdentificacionRow(int Codigo, string Descripcion);
}
