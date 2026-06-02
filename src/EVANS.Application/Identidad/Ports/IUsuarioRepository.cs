using EVANS.Domain.Identidad;

namespace EVANS.Application.Identidad.Ports;

public interface IUsuarioRepository
{
    Task<Usuario?> AuthenticateAsync(
        string nombreUsuario,
        string clave,
        CancellationToken cancellationToken = default);

    Task<CuentaUsuario?> GetByIdAsync(int codigo, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CuentaUsuario>> SearchAsync(
        string? nombreCompletoPrefix,
        CancellationToken cancellationToken = default);

    Task<int> AddAsync(CuentaUsuario usuario, CancellationToken cancellationToken = default);

    Task UpdateAsync(CuentaUsuario usuario, CancellationToken cancellationToken = default);
}
