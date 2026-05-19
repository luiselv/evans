using EVANS.Domain.Comprobante;

namespace EVANS.Application.Comprobante.DTOs;

public record ComprobanteDto(
    int Codigo,
    string NumeroComprobante,
    TipoComprobante Tipo,
    DateTime Fecha,
    int ClienteCodigo,
    string RucODni,
    string Direccion,
    decimal Total,
    decimal IGV,
    decimal ValorVenta,
    bool Impreso,
    IReadOnlyList<DetalleComprobanteDto> Detalles);
