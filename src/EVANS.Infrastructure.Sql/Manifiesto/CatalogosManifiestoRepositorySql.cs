using Dapper;
using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Manifiesto;

/// <summary>
/// Queries the EVANS master database for all catalog data needed by the Manifiesto form.
/// Single round-trip via Dapper QueryMultiple.
/// Read-only lookups only — no writes.
/// </summary>
public sealed class CatalogosManifiestoRepositorySql(
    IEvansMasterConnectionFactory masterFactory) : ICatalogosManifiestoRepository
{
    public async Task<CatalogosManifiestoDto> ObtenerCatalogosAsync(CancellationToken ct = default)
    {
        const string sql = @"
            SELECT EMPR_CODIGO AS Codigo, ISNULL(EMPR_NOMBRE, '') AS RazonSocial
              FROM EMPRESA
             WHERE ESTA_CODIGO = 1
             ORDER BY EMPR_NOMBRE;

            SELECT VEHI_CODIGO AS Codigo, ISNULL(VEHI_PLACA, '') AS Placa
              FROM VEHICULO
             WHERE ESTA_CODIGO = 1
             ORDER BY VEHI_PLACA;

            SELECT CARR_CODIGO AS Codigo, ISNULL(CARR_PLACA, '') AS Placa
              FROM CARRETA
             WHERE ESTA_CODIGO = 1
             ORDER BY CARR_PLACA;

            SELECT CHOF_CODIGO AS Codigo, ISNULL(CHOF_NOMBRE, '') AS NombreCompleto
              FROM CHOFER
             WHERE ESTA_CODIGO = 1
             ORDER BY CHOF_NOMBRE;

            SELECT ESTA_CODIGO AS Codigo, ISNULL(ESTA_DESCRIPCION, '') AS Descripcion
              FROM ESTADO
             ORDER BY ESTA_DESCRIPCION;";

        using var conn = masterFactory.Create();
        await conn.OpenAsync(ct);

        using var multi = await conn.QueryMultipleAsync(
            new CommandDefinition(sql, cancellationToken: ct));

        var transportistas = (await multi.ReadAsync<TransportistaDto>()).ToList();
        var vehiculos      = (await multi.ReadAsync<VehiculoDto>()).ToList();
        var carretas       = (await multi.ReadAsync<CarretaDto>()).ToList();
        var choferes       = (await multi.ReadAsync<ChoferDto>()).ToList();
        var estados        = (await multi.ReadAsync<EstadoDto>()).ToList();

        return new CatalogosManifiestoDto(
            Transportistas: transportistas,
            Vehiculos: vehiculos,
            Carretas: carretas,
            Choferes: choferes,
            Estados: estados);
    }
}
