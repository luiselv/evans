using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Shared.DTOs;

namespace EVANS.Application.Identidad.Ports;

public interface ICurrentSession
{
    bool IsAuthenticated { get; }
    SesionActualDto? Current { get; }

    void Start(UsuarioSesionDto usuario, ParametrosDto parametros, int year);
    void Clear();
}
