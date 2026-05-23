using Dapper;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Recepcion;

/// <summary>
/// Read-only lookups for the Recepcion catalogs from the EVANS master database.
/// No UoW — each method opens its own connection.
/// Mirrors CatalogosManifiestoRepositorySql pattern.
/// </summary>
public sealed class CatalogosRecepcionRepositorySql : ICatalogosRecepcionRepository
{
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public CatalogosRecepcionRepositorySql(IEvansMasterConnectionFactory masterFactory)
        => _masterFactory = masterFactory;

    public async Task<IReadOnlyList<ClienteLookupDto>> ListarClientesAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT CLIE_CODIGO AS Codigo, ISNULL(CLIE_NOMBRE, '') AS Nombre
            FROM CLIENTE
            ORDER BY CLIE_NOMBRE";

        using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<ClienteLookupDto>(
            new CommandDefinition(sql, cancellationToken: ct));

        return rows.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<DestinoLookupDto>> ListarDestinosAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT DEST_CODIGO AS Codigo, ISNULL(DEST_NOMBRE, '') AS Nombre
            FROM DESTINO
            WHERE ESTA_CODIGO = 1
            ORDER BY DEST_NOMBRE";

        using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<DestinoLookupDto>(
            new CommandDefinition(sql, cancellationToken: ct));

        return rows.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<EstadoLookupDto>> ListarEstadosAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT ESTA_CODIGO AS Codigo, ISNULL(ESTA_DESCRIPCION, '') AS Nombre
            FROM ESTADO
            ORDER BY ESTA_DESCRIPCION";

        using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<EstadoLookupDto>(
            new CommandDefinition(sql, cancellationToken: ct));

        return rows.ToList().AsReadOnly();
    }
}
