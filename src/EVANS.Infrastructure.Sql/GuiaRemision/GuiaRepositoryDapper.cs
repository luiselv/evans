using System.Data;
using System.Text;
using Dapper;
using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.GuiaRemision;
using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Data.SqlClient;

namespace EVANS.Infrastructure.Sql.GuiaRemision;

/// <summary>
/// Dapper implementation of IGuiaRepository.
/// Write methods cast IUnitOfWork to SqlUnitOfWork to access Connection+Transaction.
/// This cast is safe because both caller and callee live in EVANS.Infrastructure.Sql.
/// Read methods open their own connection — no transaction scope.
/// </summary>
public sealed class GuiaRepositoryDapper : IGuiaRepository
{
    private readonly IYearlyTransactionalConnectionFactory _yearlyFactory;
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public GuiaRepositoryDapper(
        IYearlyTransactionalConnectionFactory yearlyFactory,
        IEvansMasterConnectionFactory masterFactory)
    {
        _yearlyFactory = yearlyFactory;
        _masterFactory = masterFactory;
    }

    // ------------------------------------------------------------------
    // Write methods
    // ------------------------------------------------------------------

    public int Insertar(Guia guia, IUnitOfWork uow)
    {
        var sqlUow = CastUow(uow);

        const string insertGuia = @"
            INSERT INTO GuiaRemision (
                GREM_SERIE, GREM_NUMERO,
                GREM_FECHAEMISION, GREM_FECHATRASLADO,
                CLIE_REMITENTE, GREM_TIPODIRPARTIDA, GREM_DIRECCIONPARTIDA,
                CLIE_DESTINATARIO, GREM_TIPODIRDESTINO, GREM_DIRECCIONDESTINO,
                DEST_CODIGO, VEHI_CODIGO, CARR_CODIGO, CHOF_CODIGO, EMPR_CODIGO,
                ESTA_CODIGO, TICO_CODIGO,
                GREM_BULTOS, GREM_PESOTOTAL, GREM_COSTOTOTAL,
                GREM_IMPRESO, USU_CODIGO, GREM_ENVIADO, GREM_MANIFIESTO
            )
            VALUES (
                @serie, @numero,
                @fechaEmision, @fechaTraslado,
                @remitenteId, 1, @direccionPartida,
                @destinatarioId, 1, @direccionLlegada,
                @destCodigo, @vehiculoId, @carretaId, @choferId, @emprCodigo,
                @estaCodigo, @ticoCodigo,
                @bultos, @pesoTotal, @costoTotal,
                0, 1, 0, @manifiesto
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var (pesoTotal, costoTotal, bultos) = CalcularTotales(guia.Detalles);

        var codigo = sqlUow.Connection.ExecuteScalar<int>(
            insertGuia,
            new
            {
                serie = guia.Numero.Serie,
                numero = guia.Numero.Numero.ToString("D6"),
                fechaEmision = guia.Fecha,
                fechaTraslado = guia.Fecha,
                remitenteId = guia.RemitenteId,
                direccionPartida = guia.DireccionPartida.Serialize(),
                destinatarioId = guia.DestinatarioId,
                direccionLlegada = guia.DireccionLlegada.Serialize(),
                destCodigo = 1,
                vehiculoId = (object?)guia.VehiculoId ?? DBNull.Value,
                carretaId = (object?)guia.CarretaId ?? DBNull.Value,
                choferId = (object?)guia.ChoferId ?? DBNull.Value,
                emprCodigo = 1,
                estaCodigo = 1,
                ticoCodigo = 1,
                bultos,
                pesoTotal,
                costoTotal,
                manifiesto = guia.HasManifest ? 1 : 0
            },
            sqlUow.Transaction);

        InsertarDetalles(guia.Detalles, codigo, sqlUow.Connection, sqlUow.Transaction);
        return codigo;
    }

    public void Actualizar(Guia guia, IUnitOfWork uow)
    {
        var sqlUow = CastUow(uow);
        var codigo = guia.Codigo ?? throw new InvalidOperationException("Guia.Codigo must be set for Actualizar.");

        // CRITICAL: grem_serie and grem_numero are NOT included in the UPDATE (fiscal compliance).
        const string updateGuia = @"
            UPDATE GuiaRemision SET
                GREM_FECHAEMISION    = @fechaEmision,
                GREM_FECHATRASLADO   = @fechaTraslado,
                CLIE_REMITENTE       = @remitenteId,
                GREM_DIRECCIONPARTIDA = @direccionPartida,
                CLIE_DESTINATARIO    = @destinatarioId,
                GREM_DIRECCIONDESTINO = @direccionLlegada,
                VEHI_CODIGO          = @vehiculoId,
                CARR_CODIGO          = @carretaId,
                CHOF_CODIGO          = @choferId,
                GREM_BULTOS          = @bultos,
                GREM_PESOTOTAL       = @pesoTotal,
                GREM_COSTOTOTAL      = @costoTotal,
                GREM_MANIFIESTO      = @manifiesto
            WHERE GREM_CODIGO = @codigo";

        var (pesoTotal, costoTotal, bultos) = CalcularTotales(guia.Detalles);

        sqlUow.Connection.Execute(
            updateGuia,
            new
            {
                codigo,
                fechaEmision = guia.Fecha,
                fechaTraslado = guia.Fecha,
                remitenteId = guia.RemitenteId,
                direccionPartida = guia.DireccionPartida.Serialize(),
                destinatarioId = guia.DestinatarioId,
                direccionLlegada = guia.DireccionLlegada.Serialize(),
                vehiculoId = (object?)guia.VehiculoId ?? DBNull.Value,
                carretaId = (object?)guia.CarretaId ?? DBNull.Value,
                choferId = (object?)guia.ChoferId ?? DBNull.Value,
                bultos,
                pesoTotal,
                costoTotal,
                manifiesto = guia.HasManifest ? 1 : 0
            },
            sqlUow.Transaction);

        // Replace all detalles
        sqlUow.Connection.Execute(
            "DELETE FROM DetalleGuia WHERE GREM_CODIGO = @codigo",
            new { codigo },
            sqlUow.Transaction);

        InsertarDetalles(guia.Detalles, codigo, sqlUow.Connection, sqlUow.Transaction);
    }

    public void Eliminar(int codigo, IUnitOfWork uow)
    {
        var sqlUow = CastUow(uow);

        sqlUow.Connection.Execute(
            "DELETE FROM DetalleGuia WHERE GREM_CODIGO = @codigo",
            new { codigo },
            sqlUow.Transaction);

        sqlUow.Connection.Execute(
            "DELETE FROM GuiaRemision WHERE GREM_CODIGO = @codigo",
            new { codigo },
            sqlUow.Transaction);
    }

    // ------------------------------------------------------------------
    // Read methods — own connections, no transaction
    // ------------------------------------------------------------------

    public GuiaDetalleDto? ObtenerPorCodigo(int codigo, int year)
    {
        const string sql = @"
            SELECT
                G.GREM_CODIGO,
                G.GREM_SERIE,
                G.GREM_NUMERO,
                G.GREM_FECHAEMISION,
                G.CLIE_REMITENTE,
                G.CLIE_DESTINATARIO,
                G.GREM_DIRECCIONPARTIDA,
                G.GREM_DIRECCIONDESTINO,
                G.GREM_MANIFIESTO,
                G.VEHI_CODIGO,
                G.CARR_CODIGO,
                G.CHOF_CODIGO,
                G.GREM_PESOTOTAL,
                G.GREM_COSTOTOTAL,
                D.GREM_CODIGO    AS Det_GREM_CODIGO,
                D.DEGR_DESCRIPCION,
                D.DEGR_CANTIDAD,
                D.DEGR_PESO,
                D.DEGR_COSTO
            FROM GuiaRemision G
            LEFT JOIN DetalleGuia D ON D.GREM_CODIGO = G.GREM_CODIGO
            WHERE G.GREM_CODIGO = @codigo";

        using var conn = _yearlyFactory.Create(year);
        conn.Open();

        var igvRate = ObtenerIgvRate();

        var guiaDict = new Dictionary<int, (GuiaRow Header, List<DetalleRow> Detalles)>();

        conn.Query<GuiaRow, DetalleRow?, int>(
            sql,
            (header, detalle) =>
            {
                if (!guiaDict.TryGetValue(header.GREM_CODIGO, out var entry))
                {
                    entry = (header, []);
                    guiaDict[header.GREM_CODIGO] = entry;
                }
                if (detalle?.DEGR_DESCRIPCION is not null)
                    entry.Detalles.Add(detalle);
                return header.GREM_CODIGO;
            },
            new { codigo },
            splitOn: "Det_GREM_CODIGO");

        if (!guiaDict.TryGetValue(codigo, out var result)) return null;

        var (header, detalles) = result;
        return MapToDto(header, detalles, igvRate);
    }

    public IReadOnlyList<GuiaResumenDto> Buscar(BuscarGuiasFiltro filtro, int year)
    {
        var sb = new StringBuilder(@"
            SELECT
                G.GREM_CODIGO,
                G.GREM_SERIE,
                G.GREM_NUMERO,
                G.GREM_FECHAEMISION,
                G.CLIE_REMITENTE,
                G.CLIE_DESTINATARIO,
                G.GREM_MANIFIESTO,
                G.GREM_PESOTOTAL,
                G.GREM_COSTOTOTAL
            FROM GuiaRemision G
            WHERE 1=1");

        var parameters = new DynamicParameters();

        if (filtro.Desde.HasValue)
        {
            sb.Append(" AND G.GREM_FECHAEMISION >= @desde");
            parameters.Add("desde", filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            sb.Append(" AND G.GREM_FECHAEMISION <= @hasta");
            parameters.Add("hasta", filtro.Hasta.Value.Date.AddDays(1).AddSeconds(-1));
        }

        if (filtro.ClienteId.HasValue)
        {
            sb.Append(" AND (G.CLIE_REMITENTE = @clienteId OR G.CLIE_DESTINATARIO = @clienteId)");
            parameters.Add("clienteId", filtro.ClienteId.Value);
        }

        if (filtro.EstadoId.HasValue)
        {
            sb.Append(" AND G.ESTA_CODIGO = @estadoId");
            parameters.Add("estadoId", filtro.EstadoId.Value);
        }

        sb.Append(" ORDER BY G.GREM_FECHAEMISION DESC");

        using var conn = _yearlyFactory.Create(year);
        conn.Open();

        var igvRate = ObtenerIgvRate();

        var rows = conn.Query<GuiaResumenRow>(sb.ToString(), parameters);

        return rows
            .Select(r => new GuiaResumenDto(
                Codigo: r.GREM_CODIGO,
                NumeroGuia: $"{r.GREM_SERIE}-{r.GREM_NUMERO}",
                Fecha: r.GREM_FECHAEMISION,
                RemitenteId: r.CLIE_REMITENTE,
                RemitenteNombre: string.Empty,  // Summary does not join CLIENTE for performance
                DestinatarioId: r.CLIE_DESTINATARIO,
                DestinatarioNombre: string.Empty,
                HasManifest: r.GREM_MANIFIESTO == 1,
                Igv: (decimal)igvRate))
            .ToList()
            .AsReadOnly();
    }

    // ------------------------------------------------------------------
    // Private helpers
    // ------------------------------------------------------------------

    private static SqlUnitOfWork CastUow(IUnitOfWork uow)
    {
        if (uow is not SqlUnitOfWork sqlUow)
            throw new InvalidOperationException(
                $"Expected {nameof(SqlUnitOfWork)} but got {uow.GetType().Name}. " +
                "Only SqlUnitOfWork instances created by SqlUnitOfWorkFactory are supported.");
        return sqlUow;
    }

    private static void InsertarDetalles(
        IReadOnlyList<DetalleGuia> detalles,
        int gremCodigo,
        SqlConnection conn,
        SqlTransaction tx)
    {
        const string insertDetalle = @"
            INSERT INTO DetalleGuia (
                GREM_CODIGO, DEGR_CANTIDAD, DEGR_DESCRIPCION, DEGR_PESO, DEGR_COSTO
            )
            VALUES (
                @gremCodigo, @cantidad, @descripcion, @peso, @costo
            )";

        foreach (var d in detalles)
        {
            conn.Execute(
                insertDetalle,
                new
                {
                    gremCodigo,
                    cantidad = (double)d.Cantidad,
                    descripcion = d.Descripcion,
                    peso = (double)d.Peso.Valor,
                    costo = (double)d.PrecioUnitario
                },
                tx);
        }
    }

    private static (double pesoTotal, double costoTotal, int bultos) CalcularTotales(
        IReadOnlyList<DetalleGuia> detalles)
    {
        var pesoTotal = detalles.Sum(d => (double)d.Peso.Valor * d.Cantidad);
        var costoTotal = detalles.Sum(d => (double)d.PrecioTotal);
        var bultos = detalles.Sum(d => d.Cantidad);
        return (pesoTotal, costoTotal, bultos);
    }

    private double ObtenerIgvRate()
    {
        using var conn = _masterFactory.Create();
        conn.Open();
        var rate = conn.ExecuteScalar<double?>("SELECT TOP 1 PARA_IGV FROM PARAMETROS");
        return rate ?? 0.18;
    }

    private static GuiaDetalleDto MapToDto(
        GuiaRow header,
        List<DetalleRow> detalles,
        double igvRate)
    {
        var detalleItems = detalles
            .Select(d => new DetalleGuiaItemDto(
                Codigo: null,
                Descripcion: d.DEGR_DESCRIPCION ?? string.Empty,
                PesoValor: (decimal)(d.DEGR_PESO ?? 0),
                PrecioUnitario: (decimal)(d.DEGR_COSTO ?? 0),
                PrecioTotal: (decimal)((d.DEGR_COSTO ?? 0) * (d.DEGR_CANTIDAD ?? 0)),
                Cantidad: (int)(d.DEGR_CANTIDAD ?? 0)))
            .ToList();

        return new GuiaDetalleDto(
            Codigo: header.GREM_CODIGO,
            NumeroGuia: $"{header.GREM_SERIE}-{header.GREM_NUMERO}",
            Fecha: header.GREM_FECHAEMISION,
            RemitenteId: header.CLIE_REMITENTE,
            RemitenteNombre: string.Empty,
            DestinatarioId: header.CLIE_DESTINATARIO,
            DestinatarioNombre: string.Empty,
            DireccionPartida: header.GREM_DIRECCIONPARTIDA ?? string.Empty,
            DireccionLlegada: header.GREM_DIRECCIONDESTINO ?? string.Empty,
            HasManifest: header.GREM_MANIFIESTO == 1,
            VehiculoId: header.VEHI_CODIGO,
            CarretaId: header.CARR_CODIGO,
            ChoferId: header.CHOF_CODIGO,
            Igv: (decimal)igvRate,
            Detalles: detalleItems);
    }

    // ------------------------------------------------------------------
    // Private raw row types for Dapper mapping
    // ------------------------------------------------------------------

    private sealed class GuiaRow
    {
        public int GREM_CODIGO { get; init; }
        public string? GREM_SERIE { get; init; }
        public string? GREM_NUMERO { get; init; }
        public DateTime GREM_FECHAEMISION { get; init; }
        public int CLIE_REMITENTE { get; init; }
        public int CLIE_DESTINATARIO { get; init; }
        public string? GREM_DIRECCIONPARTIDA { get; init; }
        public string? GREM_DIRECCIONDESTINO { get; init; }
        public int GREM_MANIFIESTO { get; init; }
        public int? VEHI_CODIGO { get; init; }
        public int? CARR_CODIGO { get; init; }
        public int? CHOF_CODIGO { get; init; }
        public double? GREM_PESOTOTAL { get; init; }
        public double? GREM_COSTOTOTAL { get; init; }
    }

    private sealed class DetalleRow
    {
        public int Det_GREM_CODIGO { get; init; }
        public string? DEGR_DESCRIPCION { get; init; }
        public double? DEGR_CANTIDAD { get; init; }
        public double? DEGR_PESO { get; init; }
        public double? DEGR_COSTO { get; init; }
    }

    private sealed class GuiaResumenRow
    {
        public int GREM_CODIGO { get; init; }
        public string? GREM_SERIE { get; init; }
        public string? GREM_NUMERO { get; init; }
        public DateTime GREM_FECHAEMISION { get; init; }
        public int CLIE_REMITENTE { get; init; }
        public int CLIE_DESTINATARIO { get; init; }
        public int GREM_MANIFIESTO { get; init; }
        public double? GREM_PESOTOTAL { get; init; }
        public double? GREM_COSTOTOTAL { get; init; }
    }
}
