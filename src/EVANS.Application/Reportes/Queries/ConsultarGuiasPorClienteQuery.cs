using EVANS.Application.Reportes.DTOs;
using MediatR;

namespace EVANS.Application.Reportes.Queries;

public sealed record ConsultarGuiasPorClienteQuery(GuiasPorClienteFiltro Filtro)
    : IRequest<IReadOnlyList<GuiaPorClienteDto>>;
