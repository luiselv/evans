using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using MediatR;

namespace EVANS.Application.Reportes.Handlers;

public sealed class ConsultarGuiasPorClienteQueryHandler
    : IRequestHandler<ConsultarGuiasPorClienteQuery, IReadOnlyList<GuiaPorClienteDto>>
{
    private readonly IReportesConsultaRepository _repository;

    public ConsultarGuiasPorClienteQueryHandler(IReportesConsultaRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<GuiaPorClienteDto>> Handle(
        ConsultarGuiasPorClienteQuery request,
        CancellationToken cancellationToken)
    {
        return _repository.ConsultarGuiasPorClienteAsync(
            request.Filtro,
            request.Filtro.Year,
            cancellationToken);
    }
}
