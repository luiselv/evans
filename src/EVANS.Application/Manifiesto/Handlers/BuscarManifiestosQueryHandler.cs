using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.Manifiesto.Queries;
using MediatR;

namespace EVANS.Application.Manifiesto.Handlers;

public sealed class BuscarManifiestosQueryHandler : IRequestHandler<BuscarManifiestosQuery, IReadOnlyList<ManifiestoResumenDto>>
{
    private readonly IManifiestoRepository _repo;

    public BuscarManifiestosQueryHandler(IManifiestoRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyList<ManifiestoResumenDto>> Handle(BuscarManifiestosQuery request, CancellationToken cancellationToken)
    {
        return _repo.BuscarAsync(request.Filtro, request.Filtro.Year, cancellationToken);
    }
}
