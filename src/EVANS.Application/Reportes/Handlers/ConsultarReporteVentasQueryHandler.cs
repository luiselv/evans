using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using MediatR;

namespace EVANS.Application.Reportes.Handlers;

public sealed class ConsultarReporteVentasQueryHandler
    : IRequestHandler<ConsultarReporteVentasQuery, IReadOnlyList<VentaReporteDto>>
{
    private readonly IReportesConsultaRepository _repository;

    public ConsultarReporteVentasQueryHandler(IReportesConsultaRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<VentaReporteDto>> Handle(
        ConsultarReporteVentasQuery request,
        CancellationToken cancellationToken)
    {
        return _repository.ConsultarReporteVentasAsync(
            request.Filtro,
            request.Filtro.Year,
            cancellationToken);
    }
}
