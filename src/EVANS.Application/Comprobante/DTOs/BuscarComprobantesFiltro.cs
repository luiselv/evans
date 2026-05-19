using EVANS.Domain.Comprobante;

namespace EVANS.Application.Comprobante.DTOs;

public record BuscarComprobantesFiltro(
    DateTime? Desde,
    DateTime? Hasta,
    int? ClienteCodigo,
    TipoComprobante? Tipo,
    bool? SoloImpreso);
