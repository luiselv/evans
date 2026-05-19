using EVANS.Domain.Comprobante;

namespace EVANS.Application.Comprobante.DTOs;

public record ComprobanteResumenDto(
    int Codigo,
    string NumeroComprobante,
    TipoComprobante Tipo,
    DateTime Fecha,
    int ClienteCodigo,
    decimal Total,
    bool Impreso);
