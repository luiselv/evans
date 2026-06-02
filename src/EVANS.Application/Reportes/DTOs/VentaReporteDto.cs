namespace EVANS.Application.Reportes.DTOs;

public sealed record VentaReporteDto(
    DateTime Fecha,
    int TipoCodigo,
    string Serie,
    string Numero,
    string ClienteNumeroIdentificacion,
    string ClienteNombre,
    decimal ValorVenta,
    decimal Igv,
    decimal Total);
