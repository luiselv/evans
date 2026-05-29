using EVANS.Application.Common;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using MediatR;

namespace EVANS.Application.Recepcion.Queries;

public sealed class BuscarRecepcionesQueryHandler
    : IRequestHandler<BuscarRecepcionesQuery, Result<IReadOnlyList<RecepcionListItemDto>>>
{
    private readonly IRecepcionRepository _repo;

    public BuscarRecepcionesQueryHandler(IRecepcionRepository repo) => _repo = repo;

    public async Task<Result<IReadOnlyList<RecepcionListItemDto>>> Handle(
        BuscarRecepcionesQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.BuscarPorRangoFechasAsync(request.Rango, request.Year, cancellationToken);
        return Result<IReadOnlyList<RecepcionListItemDto>>.Ok(items);
    }
}
