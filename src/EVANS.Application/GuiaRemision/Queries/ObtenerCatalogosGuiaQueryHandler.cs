using EVANS.Application.GuiaRemision.DTOs;
using EVANS.Application.GuiaRemision.Ports;
using MediatR;

namespace EVANS.Application.GuiaRemision.Queries;

public sealed class ObtenerCatalogosGuiaQueryHandler : IRequestHandler<ObtenerCatalogosGuiaQuery, CatalogosGuiaDto>
{
    private readonly ICatalogosGuiaRepository _catalogos;

    public ObtenerCatalogosGuiaQueryHandler(ICatalogosGuiaRepository catalogos)
    {
        _catalogos = catalogos;
    }

    public Task<CatalogosGuiaDto> Handle(ObtenerCatalogosGuiaQuery request, CancellationToken cancellationToken)
    {
        var result = _catalogos.ObtenerCatalogos();
        return Task.FromResult(result);
    }
}
