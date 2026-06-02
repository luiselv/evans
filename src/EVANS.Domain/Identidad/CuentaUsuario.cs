namespace EVANS.Domain.Identidad;

public sealed class CuentaUsuario
{
    private CuentaUsuario(
        int codigo,
        string nombreUsuario,
        string clave,
        string nombreCompleto,
        bool esAdministrador,
        int estadoCodigo)
    {
        if (codigo < 0)
            throw new DomainException("User code cannot be negative.");
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            throw new DomainException("Username is required.");
        if (nombreUsuario.Length > 50)
            throw new DomainException("Username cannot exceed 50 characters.");
        if (string.IsNullOrWhiteSpace(clave))
            throw new DomainException("Password is required.");
        if (clave.Length > 30)
            throw new DomainException("Password cannot exceed 30 characters.");
        if (string.IsNullOrWhiteSpace(nombreCompleto))
            throw new DomainException("Full name is required.");
        if (nombreCompleto.Length > 70)
            throw new DomainException("Full name cannot exceed 70 characters.");
        if (estadoCodigo <= 0)
            throw new DomainException("Status is required.");

        Codigo = codigo;
        NombreUsuario = nombreUsuario.Trim();
        Clave = clave;
        NombreCompleto = nombreCompleto.Trim();
        EsAdministrador = esAdministrador;
        EstadoCodigo = estadoCodigo;
    }

    public int Codigo { get; }
    public string NombreUsuario { get; private set; }
    public string Clave { get; private set; }
    public string NombreCompleto { get; private set; }
    public bool EsAdministrador { get; private set; }
    public int EstadoCodigo { get; private set; }

    public static CuentaUsuario Crear(
        string nombreUsuario,
        string clave,
        string nombreCompleto,
        bool esAdministrador,
        int estadoCodigo) =>
        new(0, nombreUsuario, clave, nombreCompleto, esAdministrador, estadoCodigo);

    public static CuentaUsuario Materializar(
        int codigo,
        string nombreUsuario,
        string clave,
        string nombreCompleto,
        bool esAdministrador,
        int estadoCodigo) =>
        new(codigo, nombreUsuario, clave, nombreCompleto, esAdministrador, estadoCodigo);

    public void Actualizar(
        string nombreUsuario,
        string clave,
        string nombreCompleto,
        bool esAdministrador,
        int estadoCodigo)
    {
        var updated = new CuentaUsuario(Codigo, nombreUsuario, clave, nombreCompleto, esAdministrador, estadoCodigo);
        NombreUsuario = updated.NombreUsuario;
        Clave = updated.Clave;
        NombreCompleto = updated.NombreCompleto;
        EsAdministrador = updated.EsAdministrador;
        EstadoCodigo = updated.EstadoCodigo;
    }
}
