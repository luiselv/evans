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
}
