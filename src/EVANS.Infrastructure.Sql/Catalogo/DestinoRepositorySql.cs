using Dapper;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Catalogo;

public sealed class DestinoRepositorySql : IRepository<Destino>
{
    private readonly IEvansMasterConnectionFactory _masterFactory;
    public DestinoRepositorySql(IEvansMasterConnectionFactory masterFactory) => _masterFactory = masterFactory;

    public async Task<Destino?> GetByIdAsync(int codigo, CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var row = await conn.QueryFirstOrDefaultAsync<DestinoRow>(new CommandDefinition(SelectSql + " WHERE DEST_CODIGO = @codigo", new { codigo }, cancellationToken: ct));
        return row is null ? null : Map(row);
    }

    public async Task<IReadOnlyList<Destino>> ListActiveAsync(CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var rows = await conn.QueryAsync<DestinoRow>(new CommandDefinition(SelectSql + " WHERE ESTA_CODIGO = @estadoActivo ORDER BY DEST_NOMBRE", new { estadoActivo = CatalogoEstado.Activo }, cancellationToken: ct));
        return rows.Select(Map).ToList().AsReadOnly();
    }

    public async Task<int> AddAsync(Destino entity, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO DESTINO (DEST_NOMBRE, DEST_DISTANCIAVIRTUAL, ESTA_CODIGO)
            VALUES (@descripcion, @distanciaVirtual, @estadoCodigo);
            SELECT CAST(SCOPE_IDENTITY() AS int);";
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        return await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task UpdateAsync(Destino entity, CancellationToken ct)
    {
        const string sql = @"
            UPDATE DESTINO
            SET DEST_NOMBRE = @descripcion, DEST_DISTANCIAVIRTUAL = @distanciaVirtual, ESTA_CODIGO = @estadoCodigo
            WHERE DEST_CODIGO = @codigo";
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteAsync(new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task DeactivateAsync(int codigo, CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteAsync(new CommandDefinition("UPDATE DESTINO SET ESTA_CODIGO = @estadoInactivo WHERE DEST_CODIGO = @codigo", new { codigo, estadoInactivo = CatalogoEstado.Inactivo }, tx, cancellationToken: ct)), ct);
    }

    private const string SelectSql = @"
        SELECT DEST_CODIGO AS Codigo, ISNULL(DEST_NOMBRE, '') AS Descripcion,
               ISNULL(DEST_DISTANCIAVIRTUAL, 0) AS DistanciaVirtual,
               ISNULL(ESTA_CODIGO, 0) AS EstadoCodigo
        FROM DESTINO";
    private static Destino Map(DestinoRow row) => Destino.Materializar(row.Codigo, row.Descripcion, row.DistanciaVirtual, row.EstadoCodigo);
    private static object ToParameters(Destino entity) => new { codigo = entity.Codigo, descripcion = entity.Descripcion, distanciaVirtual = entity.DistanciaVirtual, estadoCodigo = entity.EstadoCodigo };
    private sealed record DestinoRow(int Codigo, string Descripcion, double DistanciaVirtual, int EstadoCodigo);
}
