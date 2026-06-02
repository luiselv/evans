using EVANS.Application.Common;
using EVANS.Application.Identidad.DTOs;
using MediatR;

namespace EVANS.Application.Identidad.Commands;

public sealed record AutenticarUsuarioCommand(string NombreUsuario, string Clave)
    : IRequest<Result<UsuarioSesionDto>>;
