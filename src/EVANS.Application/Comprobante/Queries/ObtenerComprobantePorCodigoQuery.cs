using EVANS.Application.Comprobante.DTOs;
using MediatR;

namespace EVANS.Application.Comprobante.Queries;

public record ObtenerComprobantePorCodigoQuery(int Codigo) : IRequest<ComprobanteDto?>;
