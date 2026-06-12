using Dapper;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Catalogo;

public sealed class CarretaRepositorySql : IRepository<Carreta>, ICarretaMaintenanceRepository
{
    private readonly IEvansMasterConnectionFactory _masterFactory;
    public CarretaRepositorySql(IEvansMasterConnectionFactory masterFactory) => _masterFactory = masterFactory;

    public async Task<Carreta?> GetByIdAsync(int codigo, CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var row = await conn.QueryFirstOrDefaultAsync<CarretaRow>(new CommandDefinition(SelectSql + " WHERE CARR_CODIGO = @codigo", new { codigo }, cancellationToken: ct));
        return row is null ? null : Map(row);
    }

    public async Task<IReadOnlyList<Carreta>> ListActiveAsync(CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var rows = await conn.QueryAsync<CarretaRow>(new CommandDefinition(SelectSql + " WHERE ESTA_CODIGO = @estadoActivo ORDER BY CARR_PLACA", new { estadoActivo = CatalogoEstado.Activo }, cancellationToken: ct));
        return rows.Select(Map).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<Carreta>> ListAllAsync(CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var rows = await conn.QueryAsync<CarretaRow>(new CommandDefinition(SelectSql + " ORDER BY CARR_CODIGO ASC", cancellationToken: ct));
        return rows.Select(Map).ToList().AsReadOnly();
    }

    public async Task<int> AddAsync(Carreta entity, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO CARRETA (CARR_PLACA, CARR_MARCA, CARR_CERTIFICADO, EMPR_CODIGO, ESTA_CODIGO)
            VALUES (@placa, @marca, @certificado, @empresaCodigo, @estadoCodigo);
            SELECT CAST(SCOPE_IDENTITY() AS int);";
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        return await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task UpdateAsync(Carreta entity, CancellationToken ct)
    {
        const string sql = @"
            UPDATE CARRETA
            SET CARR_PLACA = @placa, CARR_MARCA = @marca, CARR_CERTIFICADO = @certificado,
                EMPR_CODIGO = @empresaCodigo, ESTA_CODIGO = @estadoCodigo
            WHERE CARR_CODIGO = @codigo";
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteAsync(new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task DeactivateAsync(int codigo, CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteAsync(new CommandDefinition("UPDATE CARRETA SET ESTA_CODIGO = @estadoInactivo WHERE CARR_CODIGO = @codigo", new { codigo, estadoInactivo = CatalogoEstado.Inactivo }, tx, cancellationToken: ct)), ct);
    }

    private const string SelectSql = @"
        SELECT CARR_CODIGO AS Codigo, ISNULL(CARR_PLACA, '') AS Placa, CARR_MARCA AS Marca,
               CARR_CERTIFICADO AS Certificado, ISNULL(EMPR_CODIGO, 0) AS EmpresaCodigo,
               ISNULL(ESTA_CODIGO, 0) AS EstadoCodigo
        FROM CARRETA";
    private static Carreta Map(CarretaRow row) => Carreta.Materializar(row.Codigo, row.Placa, row.Marca, row.Certificado, row.EmpresaCodigo, row.EstadoCodigo);
    private static object ToParameters(Carreta entity) => new { codigo = entity.Codigo, placa = entity.Placa, marca = entity.Marca, certificado = entity.Certificado, empresaCodigo = entity.EmpresaCodigo, estadoCodigo = entity.EstadoCodigo };
    private sealed record CarretaRow(int Codigo, string Placa, string? Marca, string? Certificado, int EmpresaCodigo, int EstadoCodigo);
}
