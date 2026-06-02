using EVANS.Application.Reportes.DTOs;
using MediatR;

namespace EVANS.Application.Reportes.Queries;

public sealed record ConsultarReporteVentasQuery(ReporteVentasFiltro Filtro)
    : IRequest<IReadOnlyList<VentaReporteDto>>;
