using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.DTOs;
using MediatR;

namespace EVANS.Application.GuiaRemision.Queries;

public record ObtenerGuiaPorCodigoQuery(int Codigo, int Year) : IRequest<GuiaDetalleDto?>;
