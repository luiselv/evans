using EVANS.Application.Common;
using EVANS.Application.Identidad.Ports;
using EVANS.Domain.Identidad;
using MediatR;

namespace EVANS.Application.Identidad.Commands;

public sealed class CrearUsuarioCommandHandler(IUsuarioRepository repository)
    : IRequestHandler<CrearUsuarioCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CrearUsuarioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var usuario = CuentaUsuario.Crear(
                request.NombreUsuario,
                request.Clave,
                request.NombreCompleto,
                request.EsAdministrador,
                request.EstadoCodigo);

            return Result<int>.Ok(await repository.AddAsync(usuario, cancellationToken));
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}

public sealed class ActualizarUsuarioCommandHandler(IUsuarioRepository repository)
    : IRequestHandler<ActualizarUsuarioCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ActualizarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        if (existing is null)
            return Result<bool>.Fail("Usuario not found.");

        try
        {
            existing.Actualizar(
                request.NombreUsuario,
                request.Clave,
                request.NombreCompleto,
                request.EsAdministrador,
                request.EstadoCodigo);

            await repository.UpdateAsync(existing, cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }
}
