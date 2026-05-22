using System.Text;
using Dapper;
using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Infrastructure.Sql.GuiaRemision;
using EVANS.Infrastructure.Sql.Connections;
using Agg = EVANS.Domain.Manifiesto.Manifiesto;

namespace EVANS.Infrastructure.Sql.Manifiesto;

/// <summary>
/// Dapper implementation of IManifiestoRepository.
/// Write methods cast IUnitOfWork to SqlUnitOfWork to access Connection+Transaction (yearly DB).
/// Read methods open their own yearly DB connection — no transaction scope.
/// ObtenerPorCodigoAsync uses QueryMultiple for single-round-trip header + detalle JOIN GuiaRemision.
/// Master-DB name enrichment is done in a separate call (same pattern as ComprobanteRepositoryDapper).
/// </summary>
public sealed class ManifiestoRepositoryDapper : IManifiestoRepository
{
    private readonly IYearlyTransactionalConnectionFactory _yearlyFactory;
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public ManifiestoRepositoryDapper(
        IYearlyTransactionalConnectionFactory yearlyFactory,
        IEvansMasterConnectionFactory masterFactory)
    {
        _yearlyFactory = yearlyFactory;
        _masterFactory = masterFactory;
    }

    // ------------------------------------------------------------------
    // Write methods (within yearly-DB UoW transaction)
    // ------------------------------------------------------------------

    public async Task<int> InsertarAsync(Agg manifiesto, IUnitOfWork uow, CancellationToken ct)
    {
        var sqlUow = CastUow(uow);

        const string insertMani = @"
            INSERT INTO Manifiesto (
                MANI_NUMERO, MANI_FECHA,
                EMPR_CODIGO, VEHI_CODIGO, CARR_CODIGO, CHOF_CODIGO,
                MANI_IMPORTE, MANI_NROGUIAS, MANI_PESO,
                ESTA_CODIGO, USU_CODIGO
            )
            VALUES (
                @numero, @fecha,
                @transportistaCodigo, @vehiculoCodigo, @carretaCodigo, @choferCodigo,
                @importe, @nroGuias, @peso,
                @estadoCodigo, @usuarioCodigo
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var codigo = await sqlUow.Connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                insertMani,
                new
                {
                    numero = manifiesto.Numero?.Value,
                    fecha = manifiesto.Fecha,
                    transportistaCodigo = manifiesto.TransportistaCodigo,
                    vehiculoCodigo = manifiesto.VehiculoCodigo,
                    carretaCodigo = (object?)manifiesto.CarretaCodigo ?? DBNull.Value,
                    choferCodigo = manifiesto.ChoferCodigo,
                    importe = (double)manifiesto.Importe,
                    nroGuias = manifiesto.GuiaIds.Count,
                    peso = (double)manifiesto.Peso,
                    estadoCodigo = manifiesto.EstadoCodigo,
                    usuarioCodigo = manifiesto.UsuarioCodigo
                },
                sqlUow.Transaction,
                cancellationToken: ct));

        manifiesto.SetCodigo(codigo);

        await InsertarDetalleAsync(manifiesto.GuiaIds, codigo, sqlUow, ct);

        return codigo;
    }

    public async Task ActualizarAsync(Agg manifiesto, IUnitOfWork uow, CancellationToken ct)
    {
        var sqlUow = CastUow(uow);
        var codigo = manifiesto.Codigo > 0
            ? manifiesto.Codigo
            : throw new InvalidOperationException("Manifiesto.Codigo must be set for Actualizar.");

        // NOTE: MANI_NUMERO is NOT updated — spec I-9 mandates numero is immutable after creation.
        const string updateSql = @"
            UPDATE Manifiesto SET
                MANI_FECHA        = @fecha,
                EMPR_CODIGO       = @transportistaCodigo,
                VEHI_CODIGO       = @vehiculoCodigo,
                CARR_CODIGO       = @carretaCodigo,
                CHOF_CODIGO       = @choferCodigo,
                MANI_IMPORTE      = @importe,
                MANI_NROGUIAS     = @nroGuias,
                MANI_PESO         = @peso,
                ESTA_CODIGO       = @estadoCodigo,
                USU_CODIGO        = @usuarioCodigo
            WHERE MANI_CODIGO = @codigo";

        await sqlUow.Connection.ExecuteAsync(
            new CommandDefinition(
                updateSql,
                new
                {
                    codigo,
                    fecha = manifiesto.Fecha,
                    transportistaCodigo = manifiesto.TransportistaCodigo,
                    vehiculoCodigo = manifiesto.VehiculoCodigo,
                    carretaCodigo = (object?)manifiesto.CarretaCodigo ?? DBNull.Value,
                    choferCodigo = manifiesto.ChoferCodigo,
                    importe = (double)manifiesto.Importe,
                    nroGuias = manifiesto.GuiaIds.Count,
                    peso = (double)manifiesto.Peso,
                    estadoCodigo = manifiesto.EstadoCodigo,
                    usuarioCodigo = manifiesto.UsuarioCodigo
                },
                sqlUow.Transaction,
                cancellationToken: ct));

        // Replace DetalleManifiesto rows — delete old then insert new with real MANI_CODIGO
        await sqlUow.Connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM DetalleManifiesto WHERE MANI_CODIGO = @codigo",
                new { codigo },
                sqlUow.Transaction,
                cancellationToken: ct));

        await InsertarDetalleAsync(manifiesto.GuiaIds, codigo, sqlUow, ct);
    }

    public async Task EliminarAsync(int codigo, IUnitOfWork uow, CancellationToken ct)
    {
        var sqlUow = CastUow(uow);

        // Delete child rows first — no FK constraint in DB but correct deletion order
        await sqlUow.Connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM DetalleManifiesto WHERE MANI_CODIGO = @codigo",
                new { codigo },
                sqlUow.Transaction,
                cancellationToken: ct));

        await sqlUow.Connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM Manifiesto WHERE MANI_CODIGO = @codigo",
                new { codigo },
                sqlUow.Transaction,
                cancellationToken: ct));
    }

    // ------------------------------------------------------------------
    // Read methods — own connections, no transaction
    // ------------------------------------------------------------------

    public async Task<ManifiestoDto?> ObtenerPorCodigoAsync(int codigo, int year, CancellationToken ct)
    {
        // Query 1: header (yearly DB only — no master-data JOIN possible cross-DB here)
        // Query 2: detalle JOIN GuiaRemision (single round-trip via QueryMultiple)
        const string sql = @"
            SELECT
                M.MANI_CODIGO,
                M.MANI_NUMERO,
                M.MANI_FECHA,
                M.EMPR_CODIGO,
                M.VEHI_CODIGO,
                M.CARR_CODIGO,
                M.CHOF_CODIGO,
                M.MANI_IMPORTE,
                M.MANI_PESO,
                M.MANI_NROGUIAS,
                M.ESTA_CODIGO,
                ISNULL(M.USU_CODIGO, 0) AS USU_CODIGO
            FROM Manifiesto M
            WHERE M.MANI_CODIGO = @codigo;

            SELECT
                DM.GREM_CODIGO AS GuiaId,
                ISNULL(GR.GREM_SERIE, '') + '-' + ISNULL(GR.GREM_NUMERO, '') AS NumeroGuia,
                ISNULL(GR.DEST_CODIGO, 0) AS DestinoCodigo,
                ISNULL(GR.GREM_PESOTOTAL, 0) AS Peso,
                ISNULL(GR.GREM_COSTOTOTAL, 0) AS Flete
            FROM DetalleManifiesto DM
            INNER JOIN GuiaRemision GR ON GR.GREM_CODIGO = DM.GREM_CODIGO
            WHERE DM.MANI_CODIGO = @codigo;";

        using var yearlyConn = _yearlyFactory.Create(year);
        await yearlyConn.OpenAsync(ct);

        ManifiestoHeaderRow? header;
        IReadOnlyList<ManifiestoLineaRow> lineas;

        using (var multi = await yearlyConn.QueryMultipleAsync(
            new CommandDefinition(sql, new { codigo }, cancellationToken: ct)))
        {
            header = (await multi.ReadAsync<ManifiestoHeaderRow>()).FirstOrDefault();
            lineas = (await multi.ReadAsync<ManifiestoLineaRow>()).ToList();
        }

        if (header is null) return null;

        // Collect distinct DEST_CODIGO values from lineas for a single master-DB batch lookup
        var destCodigos = lineas.Select(l => l.DestinoCodigo).Distinct().ToList();

        // Enrich with master-DB names via separate calls (non-critical — falls back to empty strings)
        var names = await ResolveMasterNamesAsync(
            header.EMPR_CODIGO, header.VEHI_CODIGO, header.CARR_CODIGO,
            header.CHOF_CODIGO, header.ESTA_CODIGO, ct);

        var destinoNames = await ResolveDestinoNamesAsync(destCodigos, ct);

        var lineaDtos = lineas
            .Select(l => new ManifiestoLineaDto(
                GuiaId: l.GuiaId,
                NumeroGuia: l.NumeroGuia ?? string.Empty,
                DestinoCodigo: l.DestinoCodigo,
                DestinoNombre: destinoNames.TryGetValue(l.DestinoCodigo, out var dn) ? dn : string.Empty,
                Peso: Round2(l.Peso),
                Flete: Round2(l.Flete)))
            .ToList();

        return new ManifiestoDto(
            Codigo: header.MANI_CODIGO,
            Numero: header.MANI_NUMERO ?? string.Empty,
            Fecha: header.MANI_FECHA,
            TransportistaCodigo: header.EMPR_CODIGO,
            TransportistaNombre: names.EmpresaNombre,
            VehiculoCodigo: header.VEHI_CODIGO,
            VehiculoPlaca: names.VehiculoPlaca,
            CarretaCodigo: header.CARR_CODIGO,
            CarretaPlaca: names.CarretaPlaca,
            ChoferCodigo: header.CHOF_CODIGO,
            ChoferNombre: names.ChoferNombre,
            Importe: Round2(header.MANI_IMPORTE),
            Peso: Round2(header.MANI_PESO),
            NroGuias: header.MANI_NROGUIAS ?? 0,
            EstadoCodigo: header.ESTA_CODIGO,
            EstadoNombre: names.EstadoNombre,
            UsuarioCodigo: header.USU_CODIGO,
            Lineas: lineaDtos);
    }

    public async Task<IReadOnlyList<ManifiestoResumenDto>> BuscarAsync(
        BuscarManifiestosFiltro filtro, int year, CancellationToken ct)
    {
        var sb = new StringBuilder(@"
            SELECT
                M.MANI_CODIGO,
                M.MANI_NUMERO,
                M.MANI_FECHA,
                M.EMPR_CODIGO,
                M.VEHI_CODIGO,
                M.CHOF_CODIGO,
                M.MANI_IMPORTE,
                M.MANI_NROGUIAS,
                M.ESTA_CODIGO
            FROM Manifiesto M
            WHERE 1=1");

        var parameters = new DynamicParameters();

        if (filtro.FechaDesde.HasValue)
        {
            sb.Append(" AND M.MANI_FECHA >= @desde");
            parameters.Add("desde", filtro.FechaDesde.Value);
        }

        if (filtro.FechaHasta.HasValue)
        {
            sb.Append(" AND M.MANI_FECHA <= @hasta");
            parameters.Add("hasta", filtro.FechaHasta.Value.Date.AddDays(1).AddSeconds(-1));
        }

        if (filtro.TransportistaCodigo.HasValue)
        {
            sb.Append(" AND M.EMPR_CODIGO = @transportistaCodigo");
            parameters.Add("transportistaCodigo", filtro.TransportistaCodigo.Value);
        }

        if (filtro.EstadoCodigo.HasValue)
        {
            sb.Append(" AND M.ESTA_CODIGO = @estadoCodigo");
            parameters.Add("estadoCodigo", filtro.EstadoCodigo.Value);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Numero))
        {
            sb.Append(" AND M.MANI_NUMERO LIKE @numero");
            parameters.Add("numero", $"%{filtro.Numero}%");
        }

        sb.Append(" ORDER BY M.MANI_FECHA DESC");

        using var conn = _yearlyFactory.Create(year);
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<ManifiestoResumenRow>(
            new CommandDefinition(sb.ToString(), parameters, cancellationToken: ct));

        return rows
            .Select(r => new ManifiestoResumenDto(
                Codigo: r.MANI_CODIGO,
                Numero: r.MANI_NUMERO ?? string.Empty,
                Fecha: r.MANI_FECHA,
                TransportistaNombre: string.Empty,
                VehiculoPlaca: string.Empty,
                ChoferNombre: string.Empty,
                Importe: Round2(r.MANI_IMPORTE),
                NroGuias: r.MANI_NROGUIAS ?? 0,
                EstadoNombre: string.Empty))
            .ToList()
            .AsReadOnly();
    }

    // ------------------------------------------------------------------
    // Private: master-DB name enrichment (non-critical — falls back on any error)
    // ------------------------------------------------------------------

    private async Task<(string EmpresaNombre, string VehiculoPlaca, string? CarretaPlaca,
        string ChoferNombre, string EstadoNombre)> ResolveMasterNamesAsync(
        int empresaCodigo, int vehiculoCodigo, int? carretaCodigo,
        int choferCodigo, int estadoCodigo, CancellationToken ct)
    {
        try
        {
            using var masterConn = _masterFactory.Create();
            await masterConn.OpenAsync(ct);

            var empresaNombre = await masterConn.ExecuteScalarAsync<string?>(
                new CommandDefinition(
                    "SELECT ISNULL(EMPR_NOMBRE,'') FROM EMPRESA WHERE EMPR_CODIGO = @codigo",
                    new { codigo = empresaCodigo }, cancellationToken: ct)) ?? string.Empty;

            var vehiculoPlaca = await masterConn.ExecuteScalarAsync<string?>(
                new CommandDefinition(
                    "SELECT ISNULL(VEHI_PLACA,'') FROM VEHICULO WHERE VEHI_CODIGO = @codigo",
                    new { codigo = vehiculoCodigo }, cancellationToken: ct)) ?? string.Empty;

            string? carretaPlaca = null;
            if (carretaCodigo.HasValue)
            {
                carretaPlaca = await masterConn.ExecuteScalarAsync<string?>(
                    new CommandDefinition(
                        "SELECT CARR_PLACA FROM CARRETA WHERE CARR_CODIGO = @codigo",
                        new { codigo = carretaCodigo.Value }, cancellationToken: ct));
            }

            var choferNombre = await masterConn.ExecuteScalarAsync<string?>(
                new CommandDefinition(
                    "SELECT ISNULL(CHOF_NOMBRE,'') FROM CHOFER WHERE CHOF_CODIGO = @codigo",
                    new { codigo = choferCodigo }, cancellationToken: ct)) ?? string.Empty;

            var estadoNombre = await masterConn.ExecuteScalarAsync<string?>(
                new CommandDefinition(
                    "SELECT ISNULL(ESTA_DESCRIPCION,'') FROM ESTADO WHERE ESTA_CODIGO = @codigo",
                    new { codigo = estadoCodigo }, cancellationToken: ct)) ?? string.Empty;

            return (empresaNombre, vehiculoPlaca, carretaPlaca, choferNombre, estadoNombre);
        }
        catch
        {
            return (string.Empty, string.Empty, null, string.Empty, string.Empty);
        }
    }

    /// <summary>
    /// Resolves DEST_NOMBRE for a batch of DEST_CODIGO values from the master DB.
    /// Returns a dictionary keyed by DEST_CODIGO. Falls back to empty on any error.
    /// </summary>
    private async Task<Dictionary<int, string>> ResolveDestinoNamesAsync(
        IReadOnlyList<int> destCodigos, CancellationToken ct)
    {
        if (destCodigos.Count == 0)
            return new Dictionary<int, string>();

        try
        {
            using var masterConn = _masterFactory.Create();
            await masterConn.OpenAsync(ct);

            var rows = await masterConn.QueryAsync<(int Codigo, string Nombre)>(
                new CommandDefinition(
                    "SELECT DEST_CODIGO AS Codigo, ISNULL(DEST_NOMBRE,'') AS Nombre FROM DESTINO WHERE DEST_CODIGO IN @codigos",
                    new { codigos = destCodigos },
                    cancellationToken: ct));

            return rows.ToDictionary(r => r.Codigo, r => r.Nombre);
        }
        catch
        {
            return new Dictionary<int, string>();
        }
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

    private static async Task InsertarDetalleAsync(
        IReadOnlyList<int> guiaIds,
        int maniCodigo,
        SqlUnitOfWork sqlUow,
        CancellationToken ct)
    {
        const string insertDetalle = @"
            INSERT INTO DetalleManifiesto (MANI_CODIGO, GREM_CODIGO)
            VALUES (@maniCodigo, @guiaCodigo)";

        foreach (var guiaId in guiaIds)
        {
            await sqlUow.Connection.ExecuteAsync(
                new CommandDefinition(
                    insertDetalle,
                    new { maniCodigo, guiaCodigo = guiaId },
                    sqlUow.Transaction,
                    cancellationToken: ct));
        }
    }

    private static decimal Round2(double? value) =>
        Math.Round((decimal)(value ?? 0.0), 2, MidpointRounding.AwayFromZero);

    // ------------------------------------------------------------------
    // Private raw row types for Dapper mapping
    // ------------------------------------------------------------------

    private sealed class ManifiestoHeaderRow
    {
        public int MANI_CODIGO { get; init; }
        public string? MANI_NUMERO { get; init; }
        public DateTime MANI_FECHA { get; init; }
        public int EMPR_CODIGO { get; init; }
        public int VEHI_CODIGO { get; init; }
        public int? CARR_CODIGO { get; init; }
        public int CHOF_CODIGO { get; init; }
        public double? MANI_IMPORTE { get; init; }
        public double? MANI_PESO { get; init; }
        public int? MANI_NROGUIAS { get; init; }
        public int ESTA_CODIGO { get; init; }
        public int USU_CODIGO { get; init; }
    }

    private sealed class ManifiestoLineaRow
    {
        public int GuiaId { get; init; }
        public string? NumeroGuia { get; init; }
        public int DestinoCodigo { get; init; }
        public double? Peso { get; init; }
        public double? Flete { get; init; }
    }

    private sealed class ManifiestoResumenRow
    {
        public int MANI_CODIGO { get; init; }
        public string? MANI_NUMERO { get; init; }
        public DateTime MANI_FECHA { get; init; }
        public int? EMPR_CODIGO { get; init; }
        public int? VEHI_CODIGO { get; init; }
        public int? CHOF_CODIGO { get; init; }
        public double? MANI_IMPORTE { get; init; }
        public int? MANI_NROGUIAS { get; init; }
        public int? ESTA_CODIGO { get; init; }
    }
}
