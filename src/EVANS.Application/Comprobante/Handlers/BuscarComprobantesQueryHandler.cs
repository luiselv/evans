using EVANS.Application.Comprobante.DTOs;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.Comprobante.Queries;
using MediatR;

namespace EVANS.Application.Comprobante.Handlers;

public sealed class BuscarComprobantesQueryHandler
    : IRequestHandler<BuscarComprobantesQuery, IReadOnlyList<ComprobanteResumenDto>>
{
    private readonly IComprobanteRepository _repo;

    public BuscarComprobantesQueryHandler(IComprobanteRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyList<ComprobanteResumenDto>> Handle(
        BuscarComprobantesQuery request,
        CancellationToken cancellationToken)
    {
        var filtro = new BuscarComprobantesFiltro(
            request.Desde,
            request.Hasta,
            request.ClienteCodigo,
            request.Tipo,
            request.SoloImpreso);

        var result = _repo.Buscar(filtro);
        return Task.FromResult(result);
    }
}
