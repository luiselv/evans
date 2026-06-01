using EVANS.Domain.Identidad;

namespace EVANS.Application.Identidad.Ports;

public interface IUsuarioRepository
{
    Task<Usuario?> AuthenticateAsync(
        string nombreUsuario,
        string clave,
        CancellationToken cancellationToken = default);
}
