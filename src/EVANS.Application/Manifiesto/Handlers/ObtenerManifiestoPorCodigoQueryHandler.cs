using EVANS.Application.Manifiesto.DTOs;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.Manifiesto.Queries;
using MediatR;

namespace EVANS.Application.Manifiesto.Handlers;

public sealed class ObtenerManifiestoPorCodigoQueryHandler : IRequestHandler<ObtenerManifiestoPorCodigoQuery, ManifiestoDto?>
{
    private readonly IManifiestoRepository _repo;

    public ObtenerManifiestoPorCodigoQueryHandler(IManifiestoRepository repo)
    {
        _repo = repo;
    }

    public Task<ManifiestoDto?> Handle(ObtenerManifiestoPorCodigoQuery request, CancellationToken cancellationToken)
    {
        return _repo.ObtenerPorCodigoAsync(request.Codigo, request.Year, cancellationToken);
    }
}
