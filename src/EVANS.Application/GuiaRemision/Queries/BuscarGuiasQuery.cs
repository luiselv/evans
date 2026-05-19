using EVANS.Application.GuiaRemision.DTOs;
using MediatR;

namespace EVANS.Application.GuiaRemision.Queries;

public record BuscarGuiasQuery(
    DateTime? Desde,
    DateTime? Hasta,
    int? ClienteId,
    int? EstadoId,
    int Year) : IRequest<IReadOnlyList<GuiaResumenDto>>;
