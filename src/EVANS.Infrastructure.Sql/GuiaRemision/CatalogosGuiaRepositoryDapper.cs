using Dapper;
using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.GuiaRemision;

/// <summary>
/// Queries the EVANS master database for all catalog data needed by the GuiaRemision form.
/// Single round-trip per query using multiple result sets via Dapper QueryMultiple.
/// </summary>
public sealed class CatalogosGuiaRepositoryDapper(
    IEvansMasterConnectionFactory masterFactory) : ICatalogosGuiaRepository
{
    public CatalogosGuiaDto ObtenerCatalogos()
    {
        const string sql = @"
            SELECT CLIE_CODIGO AS Id, CLIE_NOMBRE AS Nombre
              FROM CLIENTE
             ORDER BY CLIE_NOMBRE;

            SELECT DEST_CODIGO AS Id, DEST_NOMBRE AS Nombre
              FROM DESTINO
             WHERE ESTA_CODIGO = 1
             ORDER BY DEST_NOMBRE;

            SELECT VEHI_CODIGO AS Id,
                   ISNULL(VEHI_MARCA,'') + ' ' + ISNULL(VEHI_PLACA,'') AS Nombre
              FROM VEHICULO
             WHERE ESTA_CODIGO = 1
             ORDER BY VEHI_PLACA;

            SELECT CARR_CODIGO AS Id,
                   ISNULL(CARR_MARCA,'') + ' ' + ISNULL(CARR_PLACA,'') AS Nombre
              FROM CARRETA
             WHERE ESTA_CODIGO = 1
             ORDER BY CARR_PLACA;

            SELECT CHOF_CODIGO AS Id, CHOF_NOMBRE AS Nombre
              FROM CHOFER
             WHERE ESTA_CODIGO = 1
             ORDER BY CHOF_NOMBRE;

            SELECT TICO_CODIGO AS Id, TICO_DESCRIPCION AS Nombre
              FROM TIPOCOMPROBANTE
             WHERE ESTA_CODIGO = 1
             ORDER BY TICO_DESCRIPCION;

            SELECT TOP 1 PARA_IGV FROM PARAMETROS;";

        using var conn = masterFactory.Create();
        conn.Open();

        using var multi = conn.QueryMultiple(sql);

        var clientes = multi.Read<CatalogoItemDto>().ToList();
        var destinos = multi.Read<CatalogoItemDto>().ToList();
        var vehiculos = multi.Read<CatalogoItemDto>().ToList();
        var carretas = multi.Read<CatalogoItemDto>().ToList();
        var choferes = multi.Read<CatalogoItemDto>().ToList();
        var tiposComprobante = multi.Read<CatalogoItemDto>().ToList();
        var igvRate = multi.ReadFirstOrDefault<double?>() ?? 0.18;

        return new CatalogosGuiaDto(
            Clientes: clientes,
            Destinos: destinos,
            Vehiculos: vehiculos,
            Carretas: carretas,
            Choferes: choferes,
            TiposComprobante: tiposComprobante,
            IgvRate: (decimal)igvRate);
    }
}
