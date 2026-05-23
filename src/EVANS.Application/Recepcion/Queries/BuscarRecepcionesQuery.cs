using EVANS.Application.Common;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Domain.Recepcion;
using MediatR;

namespace EVANS.Application.Recepcion.Queries;

public record BuscarRecepcionesQuery(DateRange Rango, int Year) : IRequest<Result<IReadOnlyList<RecepcionListItemDto>>>;
