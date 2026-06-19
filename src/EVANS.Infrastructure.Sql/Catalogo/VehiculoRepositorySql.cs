using Dapper;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Catalogo;

public sealed class VehiculoRepositorySql : IRepository<Vehiculo>, IVehiculoMaintenanceRepository
{
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public VehiculoRepositorySql(IEvansMasterConnectionFactory masterFactory) => _masterFactory = masterFactory;

    public async Task<Vehiculo?> GetByIdAsync(int codigo, CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var row = await conn.QueryFirstOrDefaultAsync<VehiculoRow>(new CommandDefinition(SelectSql + " WHERE VEHI_CODIGO = @codigo", new { codigo }, cancellationToken: ct));
        return row is null ? null : Map(row);
    }

    public async Task<IReadOnlyList<Vehiculo>> ListActiveAsync(CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var rows = await conn.QueryAsync<VehiculoRow>(new CommandDefinition(SelectSql + " WHERE ESTA_CODIGO = @estadoActivo ORDER BY VEHI_PLACA", new { estadoActivo = CatalogoEstado.Activo }, cancellationToken: ct));
        return rows.Select(Map).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<Vehiculo>> ListAllAsync(CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var rows = await conn.QueryAsync<VehiculoRow>(new CommandDefinition(SelectSql + " ORDER BY VEHI_CODIGO ASC", cancellationToken: ct));
        return rows.Select(Map).ToList().AsReadOnly();
    }

    public async Task<int> AddAsync(Vehiculo entity, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO VEHICULO (VEHI_MARCA, VEHI_PLACA, VEHI_CONFVEHICULAR, VEHI_CERTINSCRIPCION, EMPR_CODIGO, ESTA_CODIGO)
            VALUES (@marca, @placa, @configuracionVehicular, @certificadoInscripcion, @empresaCodigo, @estadoCodigo);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        return await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task UpdateAsync(Vehiculo entity, CancellationToken ct)
    {
        const string sql = @"
            UPDATE VEHICULO
            SET VEHI_MARCA = @marca,
                VEHI_PLACA = @placa,
                VEHI_CONFVEHICULAR = @configuracionVehicular,
                VEHI_CERTINSCRIPCION = @certificadoInscripcion,
                EMPR_CODIGO = @empresaCodigo,
                ESTA_CODIGO = @estadoCodigo
            WHERE VEHI_CODIGO = @codigo";
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteAsync(new CommandDefinition(sql, ToParameters(entity), tx, cancellationToken: ct)), ct);
    }

    public async Task DeactivateAsync(int codigo, CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx => conn.ExecuteAsync(new CommandDefinition("UPDATE VEHICULO SET ESTA_CODIGO = @estadoInactivo WHERE VEHI_CODIGO = @codigo", new { codigo, estadoInactivo = CatalogoEstado.Inactivo }, tx, cancellationToken: ct)), ct);
    }

    private const string SelectSql = @"
        SELECT VEHI_CODIGO AS Codigo, VEHI_MARCA AS Marca, ISNULL(VEHI_PLACA, '') AS Placa,
               ISNULL(VEHI_CONFVEHICULAR, '') AS ConfiguracionVehicular, VEHI_CERTINSCRIPCION AS CertificadoInscripcion,
               ISNULL(EMPR_CODIGO, 0) AS EmpresaCodigo, ISNULL(ESTA_CODIGO, 0) AS EstadoCodigo
        FROM VEHICULO";

    private static Vehiculo Map(VehiculoRow row) => Vehiculo.Materializar(row.Codigo, row.Marca, row.Placa, row.ConfiguracionVehicular, row.CertificadoInscripcion, row.EmpresaCodigo, row.EstadoCodigo);
    private static object ToParameters(Vehiculo entity) => new { codigo = entity.Codigo, marca = entity.Marca, placa = entity.Placa, configuracionVehicular = entity.ConfiguracionVehicular, certificadoInscripcion = entity.CertificadoInscripcion, empresaCodigo = entity.EmpresaCodigo, estadoCodigo = entity.EstadoCodigo };
    private sealed record VehiculoRow(int Codigo, string? Marca, string Placa, string ConfiguracionVehicular, string? CertificadoInscripcion, int EmpresaCodigo, int EstadoCodigo);
}
