using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.Comprobante.Queries;
using MediatR;

namespace EVANS.Application.Comprobante.Handlers;

public sealed class ObtenerComprobantePorCodigoQueryHandler
    : IRequestHandler<ObtenerComprobantePorCodigoQuery, ComprobanteDto?>
{
    private readonly IComprobanteRepository _repo;

    public ObtenerComprobantePorCodigoQueryHandler(IComprobanteRepository repo)
    {
        _repo = repo;
    }

    public Task<ComprobanteDto?> Handle(
        ObtenerComprobantePorCodigoQuery request,
        CancellationToken cancellationToken)
    {
        var result = _repo.ObtenerPorCodigo(request.Codigo);
        return Task.FromResult(result);
    }
}
