using EVANS.Application.Identidad.DTOs;
using MediatR;

namespace EVANS.Application.Identidad.Queries;

public sealed record ObtenerUsuarioPorCodigoQuery(int Codigo) : IRequest<UsuarioCuentaDto?>;

public sealed record BuscarUsuariosQuery(string? NombreCompletoPrefix) : IRequest<IReadOnlyList<UsuarioCuentaDto>>;
