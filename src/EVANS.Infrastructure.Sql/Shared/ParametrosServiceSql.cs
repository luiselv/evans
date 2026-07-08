using Dapper;
using EVANS.Application.Shared.DTOs;
using EVANS.Application.Shared.Ports;
using EVANS.Infrastructure.Sql.Catalogo;
using EVANS.Infrastructure.Sql.Connections;
using Microsoft.Extensions.Logging;

namespace EVANS.Infrastructure.Sql.Shared;

/// <summary>
/// SQL Server implementation of IParametrosService.
/// Reads PARA_IGV from the PARAMETROS table in the master (EVANS) database.
/// If the column is missing or returns NULL, falls back to the Peru statutory rate 0.18.
/// </summary>
public sealed class ParametrosServiceSql : IParametrosService
{
    private const decimal PeruStatutoryIgvRate = 0.18m;

    private readonly IEvansMasterConnectionFactory _masterFactory;
    private readonly ILogger<ParametrosServiceSql>? _logger;

    public ParametrosServiceSql(
        IEvansMasterConnectionFactory masterFactory,
        ILogger<ParametrosServiceSql>? logger = null)
    {
        _masterFactory = masterFactory;
        _logger        = logger;
    }

    public async Task<decimal> ObtenerIgvRateAsync(CancellationToken ct = default)
    {
        var parametros = await ObtenerParametrosAsync(ct);
        return parametros.IgvRate;
    }

    public async Task<ParametrosDto> ObtenerParametrosAsync(CancellationToken ct = default)
    {
        try
        {
            using var conn = _masterFactory.Create();
            await conn.OpenAsync(ct);

            var row = await conn.QuerySingleOrDefaultAsync<ParametrosRow>(
                new CommandDefinition(@"
                    SELECT TOP 1
                        PARA_IGV AS IgvRate,
                        ISNULL(PARA_FACTSERIE, '') AS FacturaSerie,
                        ISNULL(PARA_FACTNRO1, '') AS FacturaNro1,
                        ISNULL(PARA_FACTNRO2, '') AS FacturaNro2,
                        ISNULL(PARA_BOLSERIE, '') AS BoletaSerie,
                        ISNULL(PARA_BOLNRO1, '') AS BoletaNro1,
                        ISNULL(PARA_BOLNRO2, '') AS BoletaNro2,
                        ISNULL(PARA_GREMSERIE, '') AS GuiaRemisionSerie,
                        ISNULL(PARA_GREMNRO1, '') AS GuiaRemisionNro1,
                        ISNULL(PARA_GREMNRO2, '') AS GuiaRemisionNro2,
                        ISNULL(PARA_MANIFIESTO, '') AS Manifiesto,
                        ISNULL(PARA_REMITENTE, '') AS Remitente,
                        ISNULL(PARA_EMAILREMITENTE, '') AS EmailRemitente,
                        ISNULL(PARA_PASSREMITENTE, '') AS PassRemitente,
                        ISNULL(PARA_SMTP, '') AS Smtp,
                        ISNULL(PARA_PUERTO, 0) AS Puerto
                    FROM PARAMETROS", cancellationToken: ct));

            if (row is null)
            {
                _logger?.LogWarning("PARAMETROS returned no rows; using safe defaults.");
                return DefaultParametros();
            }

            return ToDto(row);
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex,
                "Failed to read PARAMETROS; using safe defaults.");
            return DefaultParametros();
        }
    }

    public async Task ActualizarParametrosAsync(ParametrosDto parametros, CancellationToken ct = default)
    {
        const string sql = @"
            UPDATE PARAMETROS
            SET PARA_IGV = @IgvRate,
                PARA_FACTSERIE = @FacturaSerie,
                PARA_FACTNRO1 = @FacturaNro1,
                PARA_FACTNRO2 = @FacturaNro2,
                PARA_BOLSERIE = @BoletaSerie,
                PARA_BOLNRO1 = @BoletaNro1,
                PARA_BOLNRO2 = @BoletaNro2,
                PARA_GREMSERIE = @GuiaRemisionSerie,
                PARA_GREMNRO1 = @GuiaRemisionNro1,
                PARA_GREMNRO2 = @GuiaRemisionNro2,
                PARA_MANIFIESTO = @Manifiesto,
                PARA_REMITENTE = @Remitente,
                PARA_EMAILREMITENTE = @EmailRemitente,
                PARA_PASSREMITENTE = @PassRemitente,
                PARA_SMTP = @Smtp,
                PARA_PUERTO = @Puerto";

        using var conn = _masterFactory.Create();
        await conn.OpenAsync(ct);

        await CatalogoSqlWriteScope.ExecuteAsync(conn, tx =>
            conn.ExecuteAsync(new CommandDefinition(
                sql,
                parametros,
                tx,
                cancellationToken: ct)), ct);
    }

    private static ParametrosDto ToDto(ParametrosRow row)
    {
        var igvRate = row.IgvRate is null or 0.0
            ? PeruStatutoryIgvRate
            : (decimal)row.IgvRate.Value;

        return new ParametrosDto(
            igvRate,
            row.FacturaSerie,
            row.FacturaNro1,
            row.FacturaNro2,
            row.BoletaSerie,
            row.BoletaNro1,
            row.BoletaNro2,
            row.GuiaRemisionSerie,
            row.GuiaRemisionNro1,
            row.GuiaRemisionNro2,
            row.Manifiesto,
            row.Remitente,
            row.EmailRemitente,
            row.PassRemitente,
            row.Smtp,
            row.Puerto);
    }

    private static ParametrosDto DefaultParametros() => new(
        IgvRate: PeruStatutoryIgvRate,
        FacturaSerie: string.Empty,
        FacturaNro1: string.Empty,
        FacturaNro2: string.Empty,
        BoletaSerie: string.Empty,
        BoletaNro1: string.Empty,
        BoletaNro2: string.Empty,
        GuiaRemisionSerie: string.Empty,
        GuiaRemisionNro1: string.Empty,
        GuiaRemisionNro2: string.Empty,
        Manifiesto: string.Empty,
        Remitente: string.Empty,
        EmailRemitente: string.Empty,
        PassRemitente: string.Empty,
        Smtp: string.Empty,
        Puerto: 0);

    private sealed record ParametrosRow(
        double? IgvRate,
        string FacturaSerie,
        string FacturaNro1,
        string FacturaNro2,
        string BoletaSerie,
        string BoletaNro1,
        string BoletaNro2,
        string GuiaRemisionSerie,
        string GuiaRemisionNro1,
        string GuiaRemisionNro2,
        string Manifiesto,
        string Remitente,
        string EmailRemitente,
        string PassRemitente,
        string Smtp,
        int Puerto);
}
