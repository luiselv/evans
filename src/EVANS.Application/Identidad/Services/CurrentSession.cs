using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using EVANS.Application.Shared.DTOs;

namespace EVANS.Application.Identidad.Services;

public sealed class CurrentSession : ICurrentSession
{
    public bool IsAuthenticated => Current is not null;
    public SesionActualDto? Current { get; private set; }

    public void Start(UsuarioSesionDto usuario, ParametrosDto parametros, int year)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        ArgumentNullException.ThrowIfNull(parametros);

        Current = new SesionActualDto(usuario, parametros, year);
    }

    public void Clear() => Current = null;
}
