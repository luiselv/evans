namespace EVANS.Domain.Catalogo;

public sealed class Chofer
{
    private Chofer(int codigo, string nombreCompleto, string licencia, string? telefono, string? direccion, int empresaCodigo, int estadoCodigo)
    {
        Codigo = codigo;
        NombreCompleto = RequireText(nombreCompleto, "CAT-CHF-001", "Nombre completo is required.");
        Licencia = RequireText(licencia, "CAT-CHF-002", "Licencia is required.");
        Telefono = telefono;
        Direccion = direccion;
        EmpresaCodigo = empresaCodigo;
        EstadoCodigo = estadoCodigo;
    }

    public int Codigo { get; private set; }
    public string NombreCompleto { get; private set; }
    public string Licencia { get; private set; }
    public string? Telefono { get; private set; }
    public string? Direccion { get; private set; }
    public int EmpresaCodigo { get; private set; }
    public int EstadoCodigo { get; private set; }

    public static Chofer Crear(
        string nombreCompleto,
        string licencia,
        string? telefono,
        string? direccion,
        int empresaCodigo,
        int estadoCodigo = CatalogoEstado.Activo) =>
        new(0, nombreCompleto, licencia, telefono, direccion, empresaCodigo, estadoCodigo);

    public static Chofer Materializar(int codigo, string nombreCompleto, string licencia, string? telefono, string? direccion, int empresaCodigo, int estadoCodigo) =>
        new(codigo, nombreCompleto, licencia, telefono, direccion, empresaCodigo, estadoCodigo);

    public void Deactivate() => EstadoCodigo = CatalogoEstado.Inactivo;

    private static string RequireText(string value, string code, string message) =>
        string.IsNullOrWhiteSpace(value) ? throw new DomainException(code, message) : value.Trim();
}
