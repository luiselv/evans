using Dapper;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Catalogo;

public sealed class AgenciaRepositorySql : IAgenciaRepository
{
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public AgenciaRepositorySql(IEvansMasterConnectionFactory masterFactory)
        => _masterFactory = masterFactory;

    public async Task<Agencia?> GetByIdAsync(int codigo, CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var row = await conn.QueryFirstOrDefaultAsync<AgenciaRow>(
            new CommandDefinition(SelectSql + " WHERE AGEN_CODIGO = @codigo", new { codigo }, cancellationToken: ct));
        return row is null ? null : Map(row);
    }

    public async Task<IReadOnlyList<Agencia>> ListAsync(CancellationToken ct)
    {
        await using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);
        var rows = await conn.QueryAsync<AgenciaRow>(
            new CommandDefinition(SelectSql + " ORDER BY AGEN_CODIGO", cancellationToken: ct));
        return rows.Select(Map).ToList().AsReadOnly();
    }

    private const string SelectSql = @"
        SELECT AGEN_CODIGO AS Codigo, ISNULL(AGEN_DIRECCION, '') AS Direccion,
               ISNULL(DEST_CODIGO, 0) AS DestinoCodigo, ISNULL(ESTA_CODIGO, 0) AS EstadoCodigo
        FROM AGENCIA";

    private static Agencia Map(AgenciaRow row) =>
        Agencia.Materializar(row.Codigo, row.Direccion, row.DestinoCodigo, row.EstadoCodigo);

    private sealed record AgenciaRow(int Codigo, string Direccion, int DestinoCodigo, int EstadoCodigo);
}
