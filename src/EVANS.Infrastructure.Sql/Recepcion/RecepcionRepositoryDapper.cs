using Dapper;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using EVANS.Domain.Recepcion;
using EVANS.Infrastructure.Sql.Connections;
using EVANS.Infrastructure.Sql.GuiaRemision;
using Agg = EVANS.Domain.Recepcion.Recepcion;

namespace EVANS.Infrastructure.Sql.Recepcion;

/// <summary>
/// Dapper implementation of IRecepcionRepository.
/// Write methods cast IUnitOfWork to SqlUnitOfWork to access Connection+Transaction (yearly DB).
/// Read methods open their own yearly DB connection — no transaction scope.
/// RECE_GUIAREMISION is intentionally OMITTED from all INSERT/UPDATE statements (ADR-9).
/// </summary>
public sealed class RecepcionRepositoryDapper : IRecepcionRepository
{
    private readonly IYearlyTransactionalConnectionFactory _yearlyFactory;
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public RecepcionRepositoryDapper(
        IYearlyTransactionalConnectionFactory yearlyFactory,
        IEvansMasterConnectionFactory masterFactory)
    {
        _yearlyFactory = yearlyFactory;
        _masterFactory = masterFactory;
    }

    // ------------------------------------------------------------------
    // Write methods (within yearly-DB UoW transaction)
    // ------------------------------------------------------------------

    public async Task<int> CrearAsync(Agg recepcion, IUnitOfWork uow, CancellationToken ct)
    {
        var sqlUow = CastUow(uow);

        // RECE_GUIAREMISION intentionally OMITTED — written only by IRecepcionVinculadaService
        const string insertSql = @"
            INSERT INTO Recepcion (
                RECE_FECHAEMISION, CLIE_REMITENTE,
                RECE_TIPODIRPARTIDA, RECE_DIRECCIONPARTIDA,
                CLIE_DESTINATARIO,
                RECE_TIPODIRDESTINO, RECE_DIRECCIONDESTINO,
                DEST_CODIGO, ESTA_CODIGO,
                RECE_BULTOS, RECE_PESOTOTAL, RECE_COSTOTOTAL,
                RECE_OBSERVACION, USU_CODIGO
            )
            VALUES (
                @fechaEmision, @remitenteId,
                @tipoDirPartida, @direccionPartida,
                @destinatarioId,
                @tipoDirDestino, @direccionDestino,
                @destinoId, @estadoId,
                @bultos, @pesoTotal, @costoTotal,
                @observacion, @usuarioId
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var codigo = await sqlUow.Connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                insertSql,
                new
                {
                    fechaEmision = recepcion.FechaEmision,
                    remitenteId = recepcion.RemitenteId,
                    tipoDirPartida = (int)recepcion.TipoDirPartida,
                    direccionPartida = recepcion.DireccionPartida,
                    destinatarioId = recepcion.DestinatarioId,
                    tipoDirDestino = (int)recepcion.TipoDirDestino,
                    direccionDestino = recepcion.DireccionDestino,
                    destinoId = recepcion.DestinoId,
                    estadoId = recepcion.EstadoId,
                    bultos = (object?)recepcion.Bultos ?? DBNull.Value,
                    pesoTotal = recepcion.PesoTotal.HasValue ? (object)(double)recepcion.PesoTotal.Value : DBNull.Value,
                    costoTotal = (double)recepcion.CostoTotal,
                    observacion = (object?)recepcion.Observacion ?? DBNull.Value,
                    usuarioId = recepcion.UsuarioId
                },
                sqlUow.Transaction,
                cancellationToken: ct));

        recepcion.SetCodigo(codigo);

        await InsertarDetallesAsync(recepcion.Detalles, codigo, sqlUow, ct);

        return codigo;
    }

    public async Task ActualizarAsync(Agg recepcion, IUnitOfWork uow, CancellationToken ct)
    {
        var sqlUow = CastUow(uow);
        var codigo = recepcion.Codigo > 0
            ? recepcion.Codigo
            : throw new InvalidOperationException("Recepcion.Codigo must be set for Actualizar.");

        // RECE_GUIAREMISION intentionally OMITTED from UPDATE (ADR-9)
        const string updateSql = @"
            UPDATE Recepcion SET
                RECE_FECHAEMISION     = @fechaEmision,
                CLIE_REMITENTE        = @remitenteId,
                RECE_TIPODIRPARTIDA   = @tipoDirPartida,
                RECE_DIRECCIONPARTIDA = @direccionPartida,
                CLIE_DESTINATARIO     = @destinatarioId,
                RECE_TIPODIRDESTINO   = @tipoDirDestino,
                RECE_DIRECCIONDESTINO = @direccionDestino,
                DEST_CODIGO           = @destinoId,
                ESTA_CODIGO           = @estadoId,
                RECE_BULTOS           = @bultos,
                RECE_PESOTOTAL        = @pesoTotal,
                RECE_COSTOTOTAL       = @costoTotal,
                RECE_OBSERVACION      = @observacion
            WHERE RECE_CODIGO = @codigo";

        await sqlUow.Connection.ExecuteAsync(
            new CommandDefinition(
                updateSql,
                new
                {
                    codigo,
                    fechaEmision = recepcion.FechaEmision,
                    remitenteId = recepcion.RemitenteId,
                    tipoDirPartida = (int)recepcion.TipoDirPartida,
                    direccionPartida = recepcion.DireccionPartida,
                    destinatarioId = recepcion.DestinatarioId,
                    tipoDirDestino = (int)recepcion.TipoDirDestino,
                    direccionDestino = recepcion.DireccionDestino,
                    destinoId = recepcion.DestinoId,
                    estadoId = recepcion.EstadoId,
                    bultos = (object?)recepcion.Bultos ?? DBNull.Value,
                    pesoTotal = recepcion.PesoTotal.HasValue ? (object)(double)recepcion.PesoTotal.Value : DBNull.Value,
                    costoTotal = (double)recepcion.CostoTotal,
                    observacion = (object?)recepcion.Observacion ?? DBNull.Value,
                },
                sqlUow.Transaction,
                cancellationToken: ct));

        // Delete-and-reinsert detail rows (no PK on DetalleRecepcion — ADR-3)
        await sqlUow.Connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM DetalleRecepcion WHERE RECE_CODIGO = @codigo",
                new { codigo },
                sqlUow.Transaction,
                cancellationToken: ct));

        await InsertarDetallesAsync(recepcion.Detalles, codigo, sqlUow, ct);
    }

    public async Task EliminarAsync(int codigo, IUnitOfWork uow, CancellationToken ct)
    {
        var sqlUow = CastUow(uow);

        // Delete child rows first
        await sqlUow.Connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM DetalleRecepcion WHERE RECE_CODIGO = @codigo",
                new { codigo },
                sqlUow.Transaction,
                cancellationToken: ct));

        await sqlUow.Connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM Recepcion WHERE RECE_CODIGO = @codigo",
                new { codigo },
                sqlUow.Transaction,
                cancellationToken: ct));
    }

    // ------------------------------------------------------------------
    // Read methods — own connections, no transaction
    // ------------------------------------------------------------------

    public async Task<Agg?> ObtenerPorCodigoAsync(int codigo, int year, CancellationToken ct)
    {
        const string sql = @"
            SELECT
                R.RECE_CODIGO, R.RECE_FECHAEMISION,
                R.CLIE_REMITENTE, R.RECE_TIPODIRPARTIDA, R.RECE_DIRECCIONPARTIDA,
                R.CLIE_DESTINATARIO, R.RECE_TIPODIRDESTINO, R.RECE_DIRECCIONDESTINO,
                R.DEST_CODIGO, R.ESTA_CODIGO,
                R.RECE_BULTOS, R.RECE_PESOTOTAL, R.RECE_COSTOTOTAL,
                R.RECE_GUIAREMISION, R.RECE_OBSERVACION, R.USU_CODIGO
            FROM Recepcion R
            WHERE R.RECE_CODIGO = @codigo;

            SELECT
                DERE_CANTIDAD, DERE_DESCRIPCION, DERE_PESO, DERE_UNIDAD,
                DERE_COSTO, DERE_TIPODOC, DERE_NRODOC
            FROM DetalleRecepcion
            WHERE RECE_CODIGO = @codigo;";

        using var conn = _yearlyFactory.Create(year);
        await conn.OpenAsync(ct);

        RecepcionHeaderRow? header;
        IReadOnlyList<DetalleRow> detalleRows;

        using (var multi = await conn.QueryMultipleAsync(
            new CommandDefinition(sql, new { codigo }, cancellationToken: ct)))
        {
            header = (await multi.ReadAsync<RecepcionHeaderRow>()).FirstOrDefault();
            detalleRows = (await multi.ReadAsync<DetalleRow>()).ToList();
        }

        if (header is null) return null;

        var detalles = detalleRows
            .Select(d => DetalleRecepcion.Crear(
                (decimal)(d.DERE_CANTIDAD ?? 0),
                d.DERE_DESCRIPCION ?? string.Empty,
                (decimal)(d.DERE_PESO ?? 0),
                d.DERE_UNIDAD ?? string.Empty,
                (decimal)(d.DERE_COSTO ?? 0),
                d.DERE_TIPODOC ?? string.Empty,
                d.DERE_NRODOC ?? string.Empty))
            .ToList()
            .AsReadOnly();

        var agg = Agg.Materializar(
            codigo: header.RECE_CODIGO,
            fechaEmision: header.RECE_FECHAEMISION,
            remitenteId: header.CLIE_REMITENTE,
            tipoDirPartida: (TipoDireccion)header.RECE_TIPODIRPARTIDA,
            direccionPartida: header.RECE_DIRECCIONPARTIDA ?? string.Empty,
            destinatarioId: header.CLIE_DESTINATARIO,
            tipoDirDestino: (TipoDireccion)header.RECE_TIPODIRDESTINO,
            direccionDestino: header.RECE_DIRECCIONDESTINO ?? string.Empty,
            destinoId: header.DEST_CODIGO,
            estadoId: header.ESTA_CODIGO,
            bultos: header.RECE_BULTOS,
            pesoTotal: header.RECE_PESOTOTAL.HasValue ? (decimal?)((decimal)header.RECE_PESOTOTAL.Value) : null,
            costoTotal: (decimal)(header.RECE_COSTOTOTAL ?? 0),
            guiaRemisionVinculada: header.RECE_GUIAREMISION,
            observacion: header.RECE_OBSERVACION,
            usuarioId: header.USU_CODIGO,
            detalles: detalles);

        agg.SetGuiaRemisionVinculada(header.RECE_GUIAREMISION);

        return agg;
    }

    public async Task<IReadOnlyList<RecepcionListItemDto>> BuscarPorRangoFechasAsync(
        DateRange rango, int year, CancellationToken ct)
    {
        const string sql = @"
            SELECT
                RECE_CODIGO AS Codigo,
                RECE_FECHAEMISION AS Fecha,
                CLIE_REMITENTE AS RemitenteCodigo,
                CLIE_DESTINATARIO AS DestinatarioCodigo,
                DEST_CODIGO AS DestinoCodigo,
                ESTA_CODIGO AS EstadoCodigo,
                RECE_COSTOTOTAL AS CostoTotal,
                RECE_GUIAREMISION AS GuiaRemisionVinculada
            FROM Recepcion
            WHERE RECE_FECHAEMISION >= @inicio
              AND RECE_FECHAEMISION <= @fin
            ORDER BY RECE_FECHAEMISION DESC";

        using var conn = _yearlyFactory.Create(year);
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<RecepcionResumenRow>(
            new CommandDefinition(sql, new { inicio = rango.Inicio, fin = rango.Fin }, cancellationToken: ct));

        return rows
            .Select(r => new RecepcionListItemDto(
                Codigo: r.Codigo,
                Fecha: r.Fecha,
                RemitenteCodigo: r.RemitenteCodigo,
                RemitenteNombre: string.Empty,
                DestinatarioCodigo: r.DestinatarioCodigo,
                DestinatarioNombre: string.Empty,
                DestinoCodigo: r.DestinoCodigo,
                DestinoNombre: string.Empty,
                EstadoCodigo: r.EstadoCodigo,
                EstadoNombre: string.Empty,
                CostoTotal: Round2(r.CostoTotal),
                GuiaRemisionVinculada: r.GuiaRemisionVinculada))
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
                "Only SqlUnitOfWork instances are supported.");
        return sqlUow;
    }

    private static async Task InsertarDetallesAsync(
        IReadOnlyList<DetalleRecepcion> detalles,
        int recepcionCodigo,
        SqlUnitOfWork sqlUow,
        CancellationToken ct)
    {
        const string insertDetalle = @"
            INSERT INTO DetalleRecepcion (
                RECE_CODIGO, DERE_CANTIDAD, DERE_DESCRIPCION, DERE_PESO,
                DERE_UNIDAD, DERE_COSTO, DERE_TIPODOC, DERE_NRODOC
            )
            VALUES (
                @recepcionCodigo, @cantidad, @descripcion, @peso,
                @unidad, @costo, @tipoDoc, @nroDoc
            )";

        foreach (var d in detalles)
        {
            await sqlUow.Connection.ExecuteAsync(
                new CommandDefinition(
                    insertDetalle,
                    new
                    {
                        recepcionCodigo,
                        cantidad = (double)d.Cantidad,
                        descripcion = d.Descripcion,
                        peso = (double)d.Peso,
                        unidad = d.Unidad,
                        costo = (double)d.Costo,
                        tipoDoc = d.TipoDoc,
                        nroDoc = d.NroDoc
                    },
                    sqlUow.Transaction,
                    cancellationToken: ct));
        }
    }

    private static decimal Round2(double? value) =>
        Math.Round((decimal)(value ?? 0.0), 2, MidpointRounding.AwayFromZero);

    // ------------------------------------------------------------------
    // Private row types
    // ------------------------------------------------------------------

    private sealed class RecepcionHeaderRow
    {
        public int RECE_CODIGO { get; init; }
        public DateTime RECE_FECHAEMISION { get; init; }
        public int CLIE_REMITENTE { get; init; }
        public int RECE_TIPODIRPARTIDA { get; init; }
        public string? RECE_DIRECCIONPARTIDA { get; init; }
        public int CLIE_DESTINATARIO { get; init; }
        public int RECE_TIPODIRDESTINO { get; init; }
        public string? RECE_DIRECCIONDESTINO { get; init; }
        public int DEST_CODIGO { get; init; }
        public int ESTA_CODIGO { get; init; }
        public int? RECE_BULTOS { get; init; }
        public double? RECE_PESOTOTAL { get; init; }
        public double? RECE_COSTOTOTAL { get; init; }
        public string? RECE_GUIAREMISION { get; init; }
        public string? RECE_OBSERVACION { get; init; }
        public int USU_CODIGO { get; init; }
    }

    private sealed class DetalleRow
    {
        public double? DERE_CANTIDAD { get; init; }
        public string? DERE_DESCRIPCION { get; init; }
        public double? DERE_PESO { get; init; }
        public string? DERE_UNIDAD { get; init; }
        public double? DERE_COSTO { get; init; }
        public string? DERE_TIPODOC { get; init; }
        public string? DERE_NRODOC { get; init; }
    }

    private sealed class RecepcionResumenRow
    {
        public int Codigo { get; init; }
        public DateTime Fecha { get; init; }
        public int RemitenteCodigo { get; init; }
        public int DestinatarioCodigo { get; init; }
        public int DestinoCodigo { get; init; }
        public int EstadoCodigo { get; init; }
        public double? CostoTotal { get; init; }
        public string? GuiaRemisionVinculada { get; init; }
    }
}
