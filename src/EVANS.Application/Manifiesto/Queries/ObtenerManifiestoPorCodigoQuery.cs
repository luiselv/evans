using EVANS.Application.Manifiesto.DTOs;
using MediatR;

namespace EVANS.Application.Manifiesto.Queries;

public record ObtenerManifiestoPorCodigoQuery(int Codigo, int Year) : IRequest<ManifiestoDto?>;
