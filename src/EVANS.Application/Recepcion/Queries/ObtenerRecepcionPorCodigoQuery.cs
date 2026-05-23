using EVANS.Application.Common;
using EVANS.Application.Recepcion.DTOs;
using MediatR;

namespace EVANS.Application.Recepcion.Queries;

public record ObtenerRecepcionPorCodigoQuery(int Codigo, int Year) : IRequest<Result<RecepcionDetalleDto?>>;
