using EVANS.Domain.Shared;

namespace EVANS.Domain.Catalogo;

public sealed class Empresa
{
    private Empresa(int codigo, string razonSocial, string? direccion, string? telefono, Ruc ruc, bool esPropia, int estadoCodigo)
    {
        Codigo = codigo;
        RazonSocial = RequireText(razonSocial, "CAT-EMP-001", "Empresa razon social is required.");
        Direccion = direccion;
        Telefono = telefono;
        Ruc = ruc;
        EsPropia = esPropia;
        EstadoCodigo = estadoCodigo;
    }

    public int Codigo { get; private set; }
    public string RazonSocial { get; private set; }
    public string? Direccion { get; private set; }
    public string? Telefono { get; private set; }
    public Ruc Ruc { get; private set; }
    public bool EsPropia { get; private set; }
    public int EstadoCodigo { get; private set; }

    public static Empresa Crear(string razonSocial, string? direccion, string? telefono, Ruc ruc, bool esPropia) =>
        new(0, razonSocial, direccion, telefono, ruc, esPropia, CatalogoEstado.Activo);

    public static Empresa Materializar(int codigo, string razonSocial, string? direccion, string? telefono, Ruc ruc, bool esPropia, int estadoCodigo) =>
        new(codigo, razonSocial, direccion, telefono, ruc, esPropia, estadoCodigo);

    public void Actualizar(string razonSocial, string? direccion, string? telefono, Ruc ruc, bool esPropia)
    {
        RazonSocial = RequireText(razonSocial, "CAT-EMP-001", "Empresa razon social is required.");
        Direccion = direccion;
        Telefono = telefono;
        Ruc = ruc;
        EsPropia = esPropia;
    }

    public void Deactivate() => EstadoCodigo = CatalogoEstado.Inactivo;
    internal void SetCodigo(int codigo) => Codigo = codigo;

    private static string RequireText(string value, string code, string message) =>
        string.IsNullOrWhiteSpace(value) ? throw new DomainException(code, message) : value.Trim();
}
