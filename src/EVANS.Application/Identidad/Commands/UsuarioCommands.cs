using EVANS.Application.Common;
using MediatR;

namespace EVANS.Application.Identidad.Commands;

public sealed record CrearUsuarioCommand(
    string NombreUsuario,
    string Clave,
    string NombreCompleto,
    bool EsAdministrador,
    int EstadoCodigo) : IRequest<Result<int>>;

public sealed record ActualizarUsuarioCommand(
    int Codigo,
    string NombreUsuario,
    string Clave,
    string NombreCompleto,
    bool EsAdministrador,
    int EstadoCodigo) : IRequest<Result<bool>>;
