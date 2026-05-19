using System.Text;
using Dapper;
using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.Comprobante;
using EVANS.Infrastructure.Sql.Connections;
using EVANS.Infrastructure.Sql.GuiaRemision;
using Agg = EVANS.Domain.Comprobante.Comprobante;

namespace EVANS.Infrastructure.Sql.Comprobante;

/// <summary>
/// Dapper implementation of IComprobanteRepository.
/// Write methods cast IUnitOfWork to SqlUnitOfWork to access Connection+Transaction.
/// Read methods open their own yearly DB connection — no transaction scope.
/// </summary>
public sealed class ComprobanteRepositoryDapper : IComprobanteRepository
{
    private readonly IYearlyTransactionalConnectionFactory _yearlyFactory;
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public ComprobanteRepositoryDapper(
        IYearlyTransactionalConnectionFactory yearlyFactory,
        IEvansMasterConnectionFactory masterFactory)
    {
        _yearlyFactory = yearlyFactory;
        _masterFactory = masterFactory;
    }

    // ------------------------------------------------------------------
    // Write methods
    // ------------------------------------------------------------------

    public int Insertar(Agg comprobante, IUnitOfWork unitOfWork)
    {
        var sqlUow = CastUow(unitOfWork);

        const string insertSql = @"
            INSERT INTO Comprobante (
                COMP_SERIE, COMP_NUMERO, COMP_FECHA,
                CLIE_DESTINATARIO, COMP_DIRECCION, TICO_CODIGO,
                COMP_GRT, COMP_VALORVENTA, COMP_IGV, COMP_TOTAL,
                COMP_IMPRESO
            )
            VALUES (
                @serie, @numero, @fecha,
                @clienteCodigo, @direccion, @ticoCodigo,
                @compGrt, @valorVenta, @igv, @total,
                0
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var codigo = sqlUow.Connection.ExecuteScalar<int>(
            insertSql,
            new
            {
                serie = comprobante.Numero.Serie,
                numero = comprobante.Numero.Numero,
                fecha = comprobante.Fecha,
                clienteCodigo = comprobante.ClienteCodigo,
                direccion = comprobante.Direccion,
                ticoCodigo = (int)comprobante.Tipo,
                compGrt = ResolveCompGrt(comprobante.Origen),
                valorVenta = (double)comprobante.ValorVenta,
                igv = (double)comprobante.IGV,
                total = (double)comprobante.Total
            },
            sqlUow.Transaction);

        // Assign the DB-generated identity back to the aggregate
        comprobante.SetCodigo(codigo);

        InsertarDetalles(comprobante.Detalles, codigo, sqlUow.Connection, sqlUow.Transaction);

        return codigo;
    }

    public void Actualizar(Agg comprobante, IUnitOfWork unitOfWork)
    {
        var sqlUow = CastUow(unitOfWork);
        var codigo = comprobante.Codigo
            ?? throw new InvalidOperationException("Comprobante.Codigo must be set for Actualizar.");

        // CRITICAL: COMP_SERIE and COMP_NUMERO are NOT included in the UPDATE (fiscal compliance).
        const string updateSql = @"
            UPDATE Comprobante SET
                COMP_FECHA        = @fecha,
                CLIE_DESTINATARIO = @clienteCodigo,
                COMP_DIRECCION    = @direccion,
                COMP_GRT          = @compGrt,
                COMP_VALORVENTA   = @valorVenta,
                COMP_IGV          = @igv,
                COMP_TOTAL        = @total
            WHERE COMP_CODIGO = @codigo";

        sqlUow.Connection.Execute(
            updateSql,
            new
            {
                codigo,
                fecha = comprobante.Fecha,
                clienteCodigo = comprobante.ClienteCodigo,
                direccion = comprobante.Direccion,
                compGrt = ResolveCompGrt(comprobante.Origen),
                valorVenta = (double)comprobante.ValorVenta,
                igv = (double)comprobante.IGV,
                total = (double)comprobante.Total
            },
            sqlUow.Transaction);

        // Replace all detalles
        sqlUow.Connection.Execute(
            "DELETE FROM DetalleComprobante WHERE COMP_CODIGO = @codigo",
            new { codigo },
            sqlUow.Transaction);

        InsertarDetalles(comprobante.Detalles, codigo, sqlUow.Connection, sqlUow.Transaction);
    }

    public void Eliminar(int codigo, IUnitOfWork unitOfWork)
    {
        var sqlUow = CastUow(unitOfWork);

        sqlUow.Connection.Execute(
            "DELETE FROM DetalleComprobante WHERE COMP_CODIGO = @codigo",
            new { codigo },
            sqlUow.Transaction);

        sqlUow.Connection.Execute(
            "DELETE FROM Comprobante WHERE COMP_CODIGO = @codigo",
            new { codigo },
            sqlUow.Transaction);
    }

    public void MarcarImpreso(int codigo, IUnitOfWork unitOfWork)
    {
        var sqlUow = CastUow(unitOfWork);

        sqlUow.Connection.Execute(
            "UPDATE Comprobante SET COMP_IMPRESO = 1 WHERE COMP_CODIGO = @codigo",
            new { codigo },
            sqlUow.Transaction);
    }

    // ------------------------------------------------------------------
    // Read methods — own connections, no transaction
    // ------------------------------------------------------------------

    public ComprobanteDto? ObtenerPorCodigo(int codigo)
    {
        const string sql = @"
            SELECT
                C.COMP_CODIGO,
                C.COMP_SERIE,
                C.COMP_NUMERO,
                C.COMP_FECHA,
                C.CLIE_DESTINATARIO,
                C.COMP_DIRECCION,
                C.TICO_CODIGO,
                C.COMP_GRT,
                C.COMP_VALORVENTA,
                C.COMP_IGV,
                C.COMP_TOTAL,
                C.COMP_IMPRESO,
                D.COMP_CODIGO    AS Det_COMP_CODIGO,
                D.DECO_CANTIDAD,
                D.DECO_DESCRIPCION,
                D.DECO_PRECIOUNITARIO,
                D.DECO_FLETE
            FROM Comprobante C
            LEFT JOIN DetalleComprobante D ON D.COMP_CODIGO = C.COMP_CODIGO
            WHERE C.COMP_CODIGO = @codigo";

        // Use current year connection for reads — tests override via factory
        using var conn = _yearlyFactory.CreateForCurrentYear();
        conn.Open();

        var comprobanteDict = new Dictionary<int, (ComprobanteRow Header, List<DetalleRow> Detalles)>();

        conn.Query<ComprobanteRow, DetalleRow?, int>(
            sql,
            (header, detalle) =>
            {
                if (!comprobanteDict.TryGetValue(header.COMP_CODIGO, out var entry))
                {
                    entry = (header, []);
                    comprobanteDict[header.COMP_CODIGO] = entry;
                }
                if (detalle?.DECO_DESCRIPCION is not null)
                    entry.Detalles.Add(detalle);
                return header.COMP_CODIGO;
            },
            new { codigo },
            splitOn: "Det_COMP_CODIGO");

        if (!comprobanteDict.TryGetValue(codigo, out var result)) return null;

        // Enrich with RucODni from master DB (CLIENTE table)
        var rucODni = ResolveRucODni(result.Header.CLIE_DESTINATARIO, result.Header.TICO_CODIGO);

        return MapToDto(result.Header, result.Detalles, rucODni);
    }

    /// <summary>
    /// Reads CLIE_NROIDENTIFICACION and IDEN_CODIGO from the master DB CLIENTE table.
    /// For Factura (TICO_CODIGO=1): returns RUC. For Boleta (TICO_CODIGO=2): returns DNI.
    /// Both are stored in the single CLIE_NROIDENTIFICACION column.
    /// Falls back to empty string on any failure (non-critical enrichment).
    /// </summary>
    private string ResolveRucODni(int clienteCodigo, int ticoCodigo)
    {
        try
        {
            using var masterConn = _masterFactory.Create();
            masterConn.Open();

            var row = masterConn.QueryFirstOrDefault<ClienteIdentificacionRow>(
                "SELECT CLIE_NROIDENTIFICACION, IDEN_CODIGO FROM CLIENTE WHERE CLIE_CODIGO = @codigo",
                new { codigo = clienteCodigo });

            if (row is null) return string.Empty;

            // Factura=1 uses RUC (IDEN_CODIGO=1), Boleta=2 uses DNI (IDEN_CODIGO=2).
            // Both map to the same column — IDEN_CODIGO disambiguates the type.
            return row.CLIE_NROIDENTIFICACION ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public IReadOnlyList<ComprobanteResumenDto> Buscar(BuscarComprobantesFiltro filtro)
    {
        var sb = new StringBuilder(@"
            SELECT
                C.COMP_CODIGO,
                C.COMP_SERIE,
                C.COMP_NUMERO,
                C.COMP_FECHA,
                C.CLIE_DESTINATARIO,
                C.TICO_CODIGO,
                C.COMP_TOTAL,
                C.COMP_IMPRESO
            FROM Comprobante C
            WHERE 1=1");

        var parameters = new DynamicParameters();

        if (filtro.Desde.HasValue)
        {
            sb.Append(" AND C.COMP_FECHA >= @desde");
            parameters.Add("desde", filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            sb.Append(" AND C.COMP_FECHA <= @hasta");
            parameters.Add("hasta", filtro.Hasta.Value.Date.AddDays(1).AddSeconds(-1));
        }

        if (filtro.ClienteCodigo.HasValue)
        {
            sb.Append(" AND C.CLIE_DESTINATARIO = @clienteCodigo");
            parameters.Add("clienteCodigo", filtro.ClienteCodigo.Value);
        }

        if (filtro.Tipo.HasValue)
        {
            sb.Append(" AND C.TICO_CODIGO = @ticoCodigo");
            parameters.Add("ticoCodigo", (int)filtro.Tipo.Value);
        }

        if (filtro.SoloImpreso.HasValue)
        {
            sb.Append(" AND C.COMP_IMPRESO = @impreso");
            parameters.Add("impreso", filtro.SoloImpreso.Value ? 1 : 0);
        }

        sb.Append(" ORDER BY C.COMP_FECHA DESC");

        using var conn = _yearlyFactory.CreateForCurrentYear();
        conn.Open();

        var rows = conn.Query<ComprobanteResumenRow>(sb.ToString(), parameters);

        return rows
            .Select(r => new ComprobanteResumenDto(
                Codigo: r.COMP_CODIGO,
                NumeroComprobante: $"{r.COMP_SERIE}-{r.COMP_NUMERO}",
                Tipo: (TipoComprobante)r.TICO_CODIGO,
                Fecha: r.COMP_FECHA,
                ClienteCodigo: r.CLIE_DESTINATARIO,
                Total: Round2(r.COMP_TOTAL),
                Impreso: r.COMP_IMPRESO == 1))
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

    private static string? ResolveCompGrt(OrigenComprobante origen) =>
        origen is DesdeGuia desdeGuia ? desdeGuia.GuiaRef : null;

    private static void InsertarDetalles(
        IReadOnlyList<DetalleComprobante> detalles,
        int compCodigo,
        Microsoft.Data.SqlClient.SqlConnection conn,
        Microsoft.Data.SqlClient.SqlTransaction tx)
    {
        const string insertDetalle = @"
            INSERT INTO DetalleComprobante (
                COMP_CODIGO, DECO_CANTIDAD, DECO_DESCRIPCION, DECO_PRECIOUNITARIO, DECO_FLETE
            )
            VALUES (
                @compCodigo, @cantidad, @descripcion, @precioUnitario, @flete
            )";

        foreach (var d in detalles)
        {
            conn.Execute(
                insertDetalle,
                new
                {
                    compCodigo,
                    cantidad = (double)d.Cantidad,
                    descripcion = d.Descripcion,
                    precioUnitario = (double)d.PrecioUnitario,
                    flete = (double)d.Flete
                },
                tx);
        }
    }

    private static ComprobanteDto MapToDto(ComprobanteRow header, List<DetalleRow> detalles, string rucODni = "")
    {
        var detalleItems = detalles
            .Select(d => new DetalleComprobanteDto(
                Cantidad: (int)(d.DECO_CANTIDAD ?? 0),
                Descripcion: d.DECO_DESCRIPCION ?? string.Empty,
                PrecioUnitario: Round2(d.DECO_PRECIOUNITARIO),
                Flete: Round2(d.DECO_FLETE)))
            .ToList();

        return new ComprobanteDto(
            Codigo: header.COMP_CODIGO,
            NumeroComprobante: $"{header.COMP_SERIE}-{header.COMP_NUMERO}",
            Tipo: (TipoComprobante)header.TICO_CODIGO,
            Fecha: header.COMP_FECHA,
            ClienteCodigo: header.CLIE_DESTINATARIO,
            RucODni: rucODni,
            Direccion: header.COMP_DIRECCION ?? string.Empty,
            Total: Round2(header.COMP_TOTAL),
            IGV: Round2(header.COMP_IGV),
            ValorVenta: Round2(header.COMP_VALORVENTA),
            Impreso: header.COMP_IMPRESO == 1,
            Detalles: detalleItems);
    }

    /// <summary>
    /// Converts double? (Dapper mapping of SQL FLOAT) to decimal rounded to 2dp.
    /// Math.Round with MidpointRounding.AwayFromZero avoids float→double→decimal precision loss.
    /// </summary>
    private static decimal Round2(double? value) =>
        Math.Round((decimal)(value ?? 0.0), 2, MidpointRounding.AwayFromZero);

    // ------------------------------------------------------------------
    // Private raw row types for Dapper mapping
    // ------------------------------------------------------------------

    private sealed class ComprobanteRow
    {
        public int COMP_CODIGO { get; init; }
        public string? COMP_SERIE { get; init; }
        public string? COMP_NUMERO { get; init; }
        public DateTime COMP_FECHA { get; init; }
        public int CLIE_DESTINATARIO { get; init; }
        public string? COMP_DIRECCION { get; init; }
        public int TICO_CODIGO { get; init; }
        public string? COMP_GRT { get; init; }
        public double? COMP_VALORVENTA { get; init; }
        public double? COMP_IGV { get; init; }
        public double? COMP_TOTAL { get; init; }
        public int COMP_IMPRESO { get; init; }
    }

    private sealed class DetalleRow
    {
        public int Det_COMP_CODIGO { get; init; }
        public double? DECO_CANTIDAD { get; init; }
        public string? DECO_DESCRIPCION { get; init; }
        public double? DECO_PRECIOUNITARIO { get; init; }
        public double? DECO_FLETE { get; init; }
    }

    private sealed class ComprobanteResumenRow
    {
        public int COMP_CODIGO { get; init; }
        public string? COMP_SERIE { get; init; }
        public string? COMP_NUMERO { get; init; }
        public DateTime COMP_FECHA { get; init; }
        public int CLIE_DESTINATARIO { get; init; }
        public int TICO_CODIGO { get; init; }
        public double? COMP_TOTAL { get; init; }
        public int COMP_IMPRESO { get; init; }
    }

    private sealed class ClienteIdentificacionRow
    {
        public string? CLIE_NROIDENTIFICACION { get; init; }
        public int IDEN_CODIGO { get; init; }
    }
}
