using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using EVANS.Domain.Identidad;
using MediatR;

namespace EVANS.Application.Identidad.Queries;

public sealed class ObtenerUsuarioPorCodigoQueryHandler(IUsuarioRepository repository)
    : IRequestHandler<ObtenerUsuarioPorCodigoQuery, UsuarioCuentaDto?>
{
    public async Task<UsuarioCuentaDto?> Handle(
        ObtenerUsuarioPorCodigoQuery request,
        CancellationToken cancellationToken)
    {
        var usuario = await repository.GetByIdAsync(request.Codigo, cancellationToken);
        return usuario is null ? null : ToDto(usuario);
    }

    private static UsuarioCuentaDto ToDto(CuentaUsuario usuario) => new(
        usuario.Codigo,
        usuario.NombreUsuario,
        usuario.Clave,
        usuario.NombreCompleto,
        usuario.EsAdministrador,
        usuario.EstadoCodigo);
}

public sealed class BuscarUsuariosQueryHandler(IUsuarioRepository repository)
    : IRequestHandler<BuscarUsuariosQuery, IReadOnlyList<UsuarioCuentaDto>>
{
    public async Task<IReadOnlyList<UsuarioCuentaDto>> Handle(
        BuscarUsuariosQuery request,
        CancellationToken cancellationToken)
    {
        var usuarios = await repository.SearchAsync(request.NombreCompletoPrefix, cancellationToken);
        return usuarios
            .Select(usuario => new UsuarioCuentaDto(
                usuario.Codigo,
                usuario.NombreUsuario,
                usuario.Clave,
                usuario.NombreCompleto,
                usuario.EsAdministrador,
                usuario.EstadoCodigo))
            .ToList()
            .AsReadOnly();
    }
}
