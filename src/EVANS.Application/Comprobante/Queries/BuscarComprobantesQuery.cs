using EVANS.Application.Comprobante.DTOs;
using EVANS.Domain.Comprobante;
using MediatR;

namespace EVANS.Application.Comprobante.Queries;

public record BuscarComprobantesQuery(
    DateTime? Desde,
    DateTime? Hasta,
    int? ClienteCodigo,
    TipoComprobante? Tipo,
    bool? SoloImpreso,
    int Year) : IRequest<IReadOnlyList<ComprobanteResumenDto>>;
