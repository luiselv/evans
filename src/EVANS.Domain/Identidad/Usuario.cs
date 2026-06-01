namespace EVANS.Domain.Identidad;

public sealed class Usuario
{
    private Usuario(string nombreUsuario, string nombreCompleto, bool esAdministrador)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            throw new DomainException("Username is required.");

        if (string.IsNullOrWhiteSpace(nombreCompleto))
            throw new DomainException("Full name is required.");

        NombreUsuario = nombreUsuario.Trim();
        NombreCompleto = nombreCompleto.Trim();
        EsAdministrador = esAdministrador;
    }

    public string NombreUsuario { get; }
    public string NombreCompleto { get; }
    public bool EsAdministrador { get; }

    public static Usuario Autenticado(
        string nombreUsuario,
        string nombreCompleto,
        bool esAdministrador) =>
        new(nombreUsuario, nombreCompleto, esAdministrador);
}
