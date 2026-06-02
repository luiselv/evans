using EVANS.Application.Common;
using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using MediatR;

namespace EVANS.Application.Identidad.Commands;

public sealed class AutenticarUsuarioCommandHandler(IUsuarioRepository repository)
    : IRequestHandler<AutenticarUsuarioCommand, Result<UsuarioSesionDto>>
{
    public async Task<Result<UsuarioSesionDto>> Handle(
        AutenticarUsuarioCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.NombreUsuario))
            return Result<UsuarioSesionDto>.Fail("Username is required.");

        if (string.IsNullOrWhiteSpace(request.Clave))
            return Result<UsuarioSesionDto>.Fail("Password is required.");

        var usuario = await repository.AuthenticateAsync(
            request.NombreUsuario.Trim(),
            request.Clave,
            cancellationToken);

        return usuario is null
            ? Result<UsuarioSesionDto>.Fail("Invalid username or password.")
            : Result<UsuarioSesionDto>.Ok(new UsuarioSesionDto(
                usuario.NombreUsuario,
                usuario.NombreCompleto,
                usuario.EsAdministrador));
    }
}
