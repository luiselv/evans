using Dapper;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Catalogo;

public sealed class EmpresaRepositorySql : IRepository<Empresa>, IEmpresaMaintenanceRepository
{
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public EmpresaRepositorySql(IEvansMasterConnectionFactory masterFactory)
        => _masterFactory = masterFactory;

    public async Task<Empresa?> GetByIdAsync(int codigo, CancellationToken ct)
    {
        const string sql = @"
            SELECT
                EMPR_CODIGO AS Codigo,
                ISNULL(EMPR_NOMBRE, '') AS RazonSocial,
                EMPR_DIRECCION AS Direccion,
                EMPR_TELEFONO AS Telefono,
                ISNULL(EMPR_RUC, '') AS Ruc,
                ISNULL(EMPR_PROPIEDAD, 0) AS EsPropia,
                ISNULL(ESTA_CODIGO, 0) AS EstadoCodigo
            FROM EMPRESA
            WHERE EMPR_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var row = await conn.QueryFirstOrDefaultAsync<EmpresaRow>(
            new CommandDefinition(sql, new { codigo }, cancellationToken: ct));

        return row is null ? null : Map(row);
    }

    public async Task<IReadOnlyList<Empresa>> ListActiveAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT
                EMPR_CODIGO AS Codigo,
                ISNULL(EMPR_NOMBRE, '') AS RazonSocial,
                EMPR_DIRECCION AS Direccion,
                EMPR_TELEFONO AS Telefono,
                ISNULL(EMPR_RUC, '') AS Ruc,
                ISNULL(EMPR_PROPIEDAD, 0) AS EsPropia,
                ISNULL(ESTA_CODIGO, 0) AS EstadoCodigo
            FROM EMPRESA
            WHERE ESTA_CODIGO = @estadoActivo
            ORDER BY EMPR_NOMBRE";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<EmpresaRow>(
            new CommandDefinition(sql, new { estadoActivo = CatalogoEstado.Activo }, cancellationToken: ct));

        return rows.Select(Map).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<Empresa>> ListAllAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT
                EMPR_CODIGO AS Codigo,
                ISNULL(EMPR_NOMBRE, '') AS RazonSocial,
                EMPR_DIRECCION AS Direccion,
                EMPR_TELEFONO AS Telefono,
                ISNULL(EMPR_RUC, '') AS Ruc,
                ISNULL(EMPR_PROPIEDAD, 0) AS EsPropia,
                ISNULL(ESTA_CODIGO, 0) AS EstadoCodigo
            FROM EMPRESA
            ORDER BY EMPR_CODIGO ASC";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<EmpresaRow>(
            new CommandDefinition(sql, cancellationToken: ct));

        return rows.Select(Map).ToList().AsReadOnly();
    }

    public async Task<int> AddAsync(Empresa entity, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO EMPRESA (EMPR_NOMBRE, EMPR_DIRECCION, EMPR_TELEFONO, EMPR_RUC, EMPR_PROPIEDAD, ESTA_CODIGO)
            VALUES (@razonSocial, @direccion, @telefono, @ruc, @esPropia, @estadoCodigo);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        return await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteScalarAsync<int>(
                new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task UpdateAsync(Empresa entity, CancellationToken ct)
    {
        const string sql = @"
            UPDATE EMPRESA
            SET EMPR_NOMBRE = @razonSocial,
                EMPR_DIRECCION = @direccion,
                EMPR_TELEFONO = @telefono,
                EMPR_RUC = @ruc,
                EMPR_PROPIEDAD = @esPropia,
                ESTA_CODIGO = @estadoCodigo
            WHERE EMPR_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteAsync(new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task DeactivateAsync(int codigo, CancellationToken ct)
    {
        const string sql = @"
            UPDATE EMPRESA
            SET ESTA_CODIGO = @estadoInactivo
            WHERE EMPR_CODIGO = @codigo";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteAsync(new CommandDefinition(
                sql,
                new { codigo, estadoInactivo = CatalogoEstado.Inactivo },
                tx,
                cancellationToken: ct)), ct);
    }

    private static Empresa Map(EmpresaRow row) =>
        Empresa.Materializar(
            row.Codigo,
            row.RazonSocial,
            row.Direccion,
            row.Telefono,
            Ruc.Parse(row.Ruc),
            row.EsPropia,
            row.EstadoCodigo);

    private static object ToParameters(Empresa entity) => new
    {
        codigo = entity.Codigo,
        razonSocial = entity.RazonSocial,
        direccion = entity.Direccion,
        telefono = entity.Telefono,
        ruc = entity.Ruc.Value,
        esPropia = entity.EsPropia,
        estadoCodigo = entity.EstadoCodigo
    };

    private sealed record EmpresaRow(
        int Codigo,
        string RazonSocial,
        string? Direccion,
        string? Telefono,
        string Ruc,
        bool EsPropia,
        int EstadoCodigo);
}
