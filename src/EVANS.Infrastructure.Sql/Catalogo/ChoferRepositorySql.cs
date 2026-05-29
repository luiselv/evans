using Dapper;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Catalogo;

public sealed class ChoferRepositorySql : IRepository<Chofer>
{
    private readonly IEvansMasterConnectionFactory _masterFactory;
    public ChoferRepositorySql(IEvansMasterConnectionFactory masterFactory) => _masterFactory = masterFactory;

    public async Task<Chofer?> GetByIdAsync(int codigo, CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var row = await conn.QueryFirstOrDefaultAsync<ChoferRow>(new CommandDefinition(SelectSql + " WHERE CHOF_CODIGO = @codigo", new { codigo }, cancellationToken: ct));
        return row is null ? null : Map(row);
    }

    public async Task<IReadOnlyList<Chofer>> ListActiveAsync(CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var rows = await conn.QueryAsync<ChoferRow>(new CommandDefinition(SelectSql + " WHERE ESTA_CODIGO = @estadoActivo ORDER BY CHOF_NOMBRE", new { estadoActivo = CatalogoEstado.Activo }, cancellationToken: ct));
        return rows.Select(Map).ToList().AsReadOnly();
    }

    public async Task<int> AddAsync(Chofer entity, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO CHOFER (CHOF_NOMBRE, CHOF_LICENCIA, CHOF_TELEFONO, CHOF_DIRECCION, EMPR_CODIGO, ESTA_CODIGO)
            VALUES (@nombreCompleto, @licencia, @telefono, @direccion, @empresaCodigo, @estadoCodigo);
            SELECT CAST(SCOPE_IDENTITY() AS int);";
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        return await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task UpdateAsync(Chofer entity, CancellationToken ct)
    {
        const string sql = @"
            UPDATE CHOFER
            SET CHOF_NOMBRE = @nombreCompleto, CHOF_LICENCIA = @licencia, CHOF_TELEFONO = @telefono,
                CHOF_DIRECCION = @direccion, EMPR_CODIGO = @empresaCodigo, ESTA_CODIGO = @estadoCodigo
            WHERE CHOF_CODIGO = @codigo";
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteAsync(new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task DeactivateAsync(int codigo, CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteAsync(new CommandDefinition("UPDATE CHOFER SET ESTA_CODIGO = @estadoInactivo WHERE CHOF_CODIGO = @codigo", new { codigo, estadoInactivo = CatalogoEstado.Inactivo }, tx, cancellationToken: ct)), ct);
    }

    private const string SelectSql = @"
        SELECT CHOF_CODIGO AS Codigo, ISNULL(CHOF_NOMBRE, '') AS NombreCompleto,
               ISNULL(CHOF_LICENCIA, '') AS Licencia, CHOF_TELEFONO AS Telefono,
               CHOF_DIRECCION AS Direccion, EMPR_CODIGO AS EmpresaCodigo, ESTA_CODIGO AS EstadoCodigo
        FROM CHOFER";
    private static Chofer Map(ChoferRow row) => Chofer.Materializar(row.Codigo, row.NombreCompleto, row.Licencia, row.Telefono, row.Direccion, row.EmpresaCodigo, row.EstadoCodigo);
    private static object ToParameters(Chofer entity) => new { codigo = entity.Codigo, nombreCompleto = entity.NombreCompleto, licencia = entity.Licencia, telefono = entity.Telefono, direccion = entity.Direccion, empresaCodigo = entity.EmpresaCodigo, estadoCodigo = entity.EstadoCodigo };
    private sealed record ChoferRow(int Codigo, string NombreCompleto, string Licencia, string? Telefono, string? Direccion, int EmpresaCodigo, int EstadoCodigo);
}
