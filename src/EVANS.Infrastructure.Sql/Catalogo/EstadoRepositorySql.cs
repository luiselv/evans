using Dapper;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Catalogo;

public sealed class EstadoRepositorySql : IEstadoRepository
{
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public EstadoRepositorySql(IEvansMasterConnectionFactory masterFactory)
        => _masterFactory = masterFactory;

    public async Task<Estado?> GetByIdAsync(int codigo, CancellationToken ct)
    {
        const string sql = @"
            SELECT ESTA_CODIGO AS Codigo, ISNULL(ESTA_DESCRIPCION, '') AS Descripcion
            FROM ESTADO
            WHERE ESTA_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var row = await conn.QueryFirstOrDefaultAsync<EstadoRow>(
            new CommandDefinition(sql, new { codigo }, cancellationToken: ct));

        return row is null ? null : Estado.Materializar(row.Codigo, row.Descripcion);
    }

    public async Task<IReadOnlyList<Estado>> ListAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT ESTA_CODIGO AS Codigo, ISNULL(ESTA_DESCRIPCION, '') AS Descripcion
            FROM ESTADO
            ORDER BY ESTA_DESCRIPCION";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<EstadoRow>(
            new CommandDefinition(sql, cancellationToken: ct));

        return rows
            .Select(row => Estado.Materializar(row.Codigo, row.Descripcion))
            .ToList()
            .AsReadOnly();
    }

    public async Task<int> AddAsync(Estado estado, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO ESTADO (ESTA_DESCRIPCION)
            VALUES (@descripcion);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        return await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteScalarAsync<int>(
                new CommandDefinition(sql, new { descripcion = estado.Descripcion }, tx, cancellationToken: ct)), ct);
    }

    public async Task UpdateAsync(Estado estado, CancellationToken ct)
    {
        const string sql = @"
            UPDATE ESTADO
            SET ESTA_DESCRIPCION = @descripcion
            WHERE ESTA_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteAsync(new CommandDefinition(
                sql,
                new { codigo = estado.Codigo, descripcion = estado.Descripcion },
                tx,
                cancellationToken: ct)), ct);
    }

    private sealed record EstadoRow(int Codigo, string Descripcion);
}
