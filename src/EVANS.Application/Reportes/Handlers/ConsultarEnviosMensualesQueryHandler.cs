using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using MediatR;

namespace EVANS.Application.Reportes.Handlers;

public sealed class ConsultarEnviosMensualesQueryHandler
    : IRequestHandler<ConsultarEnviosMensualesQuery, IReadOnlyList<EnvioMensualDto>>
{
    private readonly IReportesConsultaRepository _repository;

    public ConsultarEnviosMensualesQueryHandler(IReportesConsultaRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<EnvioMensualDto>> Handle(
        ConsultarEnviosMensualesQuery request,
        CancellationToken cancellationToken)
    {
        return _repository.ConsultarEnviosMensualesAsync(
            request.Filtro,
            request.Filtro.Year,
            cancellationToken);
    }
}
