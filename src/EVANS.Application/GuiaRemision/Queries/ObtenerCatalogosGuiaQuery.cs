using EVANS.Application.GuiaRemision.DTOs;
using MediatR;

namespace EVANS.Application.GuiaRemision.Queries;

public record ObtenerCatalogosGuiaQuery : IRequest<CatalogosGuiaDto>;
