using EVANS.Application.Manifiesto.DTOs;
using MediatR;

namespace EVANS.Application.Manifiesto.Queries;

public record BuscarManifiestosQuery(BuscarManifiestosFiltro Filtro) : IRequest<IReadOnlyList<ManifiestoResumenDto>>;
