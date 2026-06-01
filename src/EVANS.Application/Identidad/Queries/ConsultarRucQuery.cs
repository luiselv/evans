using EVANS.Application.Common;
using EVANS.Application.Identidad.DTOs;
using MediatR;

namespace EVANS.Application.Identidad.Queries;

public sealed record ConsultarRucQuery(string Ruc) : IRequest<Result<SunatRucDto>>;
