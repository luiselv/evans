using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Application.GuiaRemision.Ports;
using MediatR;

namespace EVANS.Application.GuiaRemision.Queries;

public sealed class BuscarGuiasQueryHandler : IRequestHandler<BuscarGuiasQuery, IReadOnlyList<GuiaResumenDto>>
{
    private readonly IGuiaRepository _repo;

    public BuscarGuiasQueryHandler(IGuiaRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyList<GuiaResumenDto>> Handle(BuscarGuiasQuery request, CancellationToken cancellationToken)
    {
        var filtro = new BuscarGuiasFiltro(request.Desde, request.Hasta, request.ClienteId, request.EstadoId);
        var result = _repo.Buscar(filtro, request.Year);
        return Task.FromResult(result);
    }
}
