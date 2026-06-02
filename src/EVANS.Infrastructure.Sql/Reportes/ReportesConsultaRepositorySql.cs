using Dapper;
using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Infrastructure.Sql.Connections;

namespace EVANS.Infrastructure.Sql.Reportes;

public sealed class ReportesConsultaRepositorySql : IReportesConsultaRepository
{
    private readonly IYearlyTransactionalConnectionFactory _yearlyFactory;
    private readonly IEvansMasterConnectionFactory _masterFactory;

    public ReportesConsultaRepositorySql(
        IYearlyTransactionalConnectionFactory yearlyFactory,
        IEvansMasterConnectionFactory masterFactory)
    {
        _yearlyFactory = yearlyFactory;
        _masterFactory = masterFactory;
    }

    public async Task<IReadOnlyList<DestinoReporteDto>> ListarDestinosActivosAsync(CancellationToken ct)
    {
        using var masterConn = _masterFactory.Create();
        await masterConn.OpenAsync(ct);

        const string sql = @"
            SELECT
                DEST_CODIGO AS Codigo,
                ISNULL(DEST_NOMBRE, '') AS Nombre
            FROM DESTINO
            WHERE ESTA_CODIGO = 1
            ORDER BY DEST_NOMBRE;";

        var rows = await masterConn.QueryAsync<DestinoReporteDto>(
            new CommandDefinition(sql, cancellationToken: ct));

        return rows.ToList();
    }

    public async Task<IReadOnlyList<EnvioMensualDto>> ConsultarEnviosMensualesAsync(
        EnviosMensualesFiltro filtro,
        int year,
        CancellationToken ct)
    {
        if (filtro.DestinoCodigos.Count == 0)
            return Array.Empty<EnvioMensualDto>();

        using var yearlyConn = _yearlyFactory.Create(year);
        await yearlyConn.OpenAsync(ct);

        const string sql = @"
            SELECT
                G.CLIE_DESTINATARIO AS ClienteCodigo,
                COUNT(G.CLIE_DESTINATARIO) AS NroGuias,
                MAX(G.GREM_FECHAEMISION) AS UltimoEnvio
            FROM GuiaRemision G
            WHERE G.GREM_FECHAEMISION >= @fechaDesde
              AND G.GREM_FECHAEMISION <= @fechaHasta
              AND G.DEST_CODIGO IN @destinoCodigos
              AND G.ESTA_CODIGO = 1
            GROUP BY G.CLIE_DESTINATARIO;";

        var rows = (await yearlyConn.QueryAsync<EnvioMensualRow>(
            new CommandDefinition(
                sql,
                new
                {
                    fechaDesde = filtro.FechaDesde,
                    fechaHasta = filtro.FechaHasta,
                    destinoCodigos = filtro.DestinoCodigos
                },
                cancellationToken: ct))).ToList();

        if (rows.Count == 0)
            return Array.Empty<EnvioMensualDto>();

        var clienteNombres = await ResolveClienteNombresAsync(
            rows.Select(r => r.ClienteCodigo).Distinct().ToList(),
            ct);

        return rows
            .Select(r => new EnvioMensualDto(
                Cliente: clienteNombres.TryGetValue(r.ClienteCodigo, out var nombre) ? nombre : string.Empty,
                NroGuias: r.NroGuias,
                UltimoEnvio: r.UltimoEnvio))
            .OrderBy(r => r.Cliente)
            .ToList();
    }

    public async Task<IReadOnlyList<GuiaPorClienteDto>> ConsultarGuiasPorClienteAsync(
        GuiasPorClienteFiltro filtro,
        int year,
        CancellationToken ct)
    {
        using var yearlyConn = _yearlyFactory.Create(year);
        await yearlyConn.OpenAsync(ct);

        const string sql = @"
            SELECT
                G.GREM_CODIGO AS Codigo,
                ISNULL(G.GREM_SERIE, '') + '-' + ISNULL(G.GREM_NUMERO, '') AS NroDoc,
                ISNULL(G.CLIE_REMITENTE, 0) AS RemitenteCodigo,
                ISNULL(G.CLIE_DESTINATARIO, 0) AS DestinatarioCodigo,
                G.GREM_FECHAEMISION AS FechaEmision,
                G.GREM_FECHATRASLADO AS FechaTraslado,
                ISNULL(G.GREM_BULTOS, 0) AS Bultos,
                CAST(ISNULL(G.GREM_COSTOTOTAL, 0) AS decimal(18, 2)) AS CostoTotal,
                CAST(CASE WHEN ISNULL(G.GREM_ENVIADO, 0) = 1 THEN 1 ELSE 0 END AS bit) AS Enviado
            FROM GuiaRemision G
            WHERE (G.CLIE_REMITENTE = @clienteCodigo OR G.CLIE_DESTINATARIO = @clienteCodigo)
              AND (
                    (G.GREM_FECHAEMISION >= @fechaDesde AND G.GREM_FECHAEMISION <= @fechaHasta)
                 OR (G.GREM_FECHATRASLADO >= @fechaDesde AND G.GREM_FECHATRASLADO <= @fechaHasta)
              )
              AND (@soloPendientes = 0 OR ISNULL(G.GREM_ENVIADO, 0) = 0)
            ORDER BY G.GREM_FECHAEMISION, G.GREM_CODIGO;";

        var rows = (await yearlyConn.QueryAsync<GuiaPorClienteRow>(
            new CommandDefinition(
                sql,
                new
                {
                    clienteCodigo = filtro.ClienteCodigo,
                    fechaDesde = filtro.FechaDesde,
                    fechaHasta = filtro.FechaHasta,
                    soloPendientes = filtro.SoloPendientes
                },
                cancellationToken: ct))).ToList();

        if (rows.Count == 0)
            return Array.Empty<GuiaPorClienteDto>();

        var clienteCodigos = rows
            .SelectMany(r => new[] { r.RemitenteCodigo, r.DestinatarioCodigo })
            .Where(codigo => codigo > 0)
            .Distinct()
            .ToList();

        var clienteNombres = await ResolveClienteNombresAsync(clienteCodigos, ct);

        return rows.Select(r => new GuiaPorClienteDto(
                r.Codigo,
                r.NroDoc,
                clienteNombres.TryGetValue(r.RemitenteCodigo, out var remitente) ? remitente : string.Empty,
                clienteNombres.TryGetValue(r.DestinatarioCodigo, out var destinatario) ? destinatario : string.Empty,
                r.FechaEmision,
                r.FechaTraslado,
                r.Bultos,
                r.CostoTotal,
                r.Enviado))
            .ToList();
    }

    private async Task<Dictionary<int, string>> ResolveClienteNombresAsync(
        IReadOnlyList<int> clienteCodigos,
        CancellationToken ct)
    {
        using var masterConn = _masterFactory.Create();
        await masterConn.OpenAsync(ct);

        var rows = await masterConn.QueryAsync<(int Codigo, string Nombre)>(
            new CommandDefinition(
                "SELECT CLIE_CODIGO AS Codigo, ISNULL(CLIE_NOMBRE, '') AS Nombre FROM CLIENTE WHERE CLIE_CODIGO IN @codigos",
                new { codigos = clienteCodigos },
                cancellationToken: ct));

        return rows.ToDictionary(r => r.Codigo, r => r.Nombre);
    }

    private sealed class EnvioMensualRow
    {
        public int ClienteCodigo { get; init; }
        public int NroGuias { get; init; }
        public DateTime UltimoEnvio { get; init; }
    }

    private sealed class GuiaPorClienteRow
    {
        public int Codigo { get; init; }
        public string NroDoc { get; init; } = string.Empty;
        public int RemitenteCodigo { get; init; }
        public int DestinatarioCodigo { get; init; }
        public DateTime FechaEmision { get; init; }
        public DateTime FechaTraslado { get; init; }
        public int Bultos { get; init; }
        public decimal CostoTotal { get; init; }
        public bool Enviado { get; init; }
    }
}
