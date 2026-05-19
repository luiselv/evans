using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Application.GuiaRemision.Ports;
using MediatR;

namespace EVANS.Application.GuiaRemision.Queries;

public sealed class ObtenerGuiaPorCodigoQueryHandler : IRequestHandler<ObtenerGuiaPorCodigoQuery, GuiaDetalleDto?>
{
    private readonly IGuiaRepository _repo;

    public ObtenerGuiaPorCodigoQueryHandler(IGuiaRepository repo)
    {
        _repo = repo;
    }

    public Task<GuiaDetalleDto?> Handle(ObtenerGuiaPorCodigoQuery request, CancellationToken cancellationToken)
    {
        var result = _repo.ObtenerPorCodigo(request.Codigo, request.Year);
        return Task.FromResult(result);
    }
}
