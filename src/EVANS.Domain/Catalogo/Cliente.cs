using EVANS.Domain.Shared;

namespace EVANS.Domain.Catalogo;

public sealed class Cliente
{
    private readonly List<Direccion> _direcciones;

    private Cliente(
        int codigo,
        string razonSocial,
        int tipoIdCodigo,
        string nroIdentificacion,
        string? telefono,
        string? fax,
        string? email,
        string? representante,
        IReadOnlyList<Direccion> direcciones)
    {
        Codigo = codigo;
        RazonSocial = razonSocial;
        TipoIdCodigo = tipoIdCodigo;
        NroIdentificacion = nroIdentificacion;
        Telefono = telefono;
        Fax = fax;
        Email = email;
        Representante = representante;
        _direcciones = direcciones.ToList();
    }

    public int Codigo { get; private set; }
    public string RazonSocial { get; private set; }
    public int TipoIdCodigo { get; private set; }
    public string NroIdentificacion { get; private set; }
    public string? Telefono { get; private set; }
    public string? Fax { get; private set; }
    public string? Email { get; private set; }
    public string? Representante { get; private set; }
    public IReadOnlyList<Direccion> Direcciones => _direcciones.AsReadOnly();

    public static Cliente Crear(
        string razonSocial,
        int tipoIdCodigo,
        string nroIdentificacion,
        int longitudRequerida,
        string? telefono,
        string? fax,
        string? email,
        string? representante,
        IReadOnlyList<Direccion> direcciones) =>
        new(0, NormalizeRazonSocial(razonSocial), tipoIdCodigo,
            ValidateNroIdentificacion(nroIdentificacion, longitudRequerida),
            telefono, fax, email, representante, CopyDirecciones(direcciones));

    public static Cliente Materializar(
        int codigo,
        string razonSocial,
        int tipoIdCodigo,
        string nroIdentificacion,
        string? telefono,
        string? fax,
        string? email,
        string? representante,
        IReadOnlyList<Direccion> direcciones) =>
        new(codigo, NormalizeRazonSocial(razonSocial), tipoIdCodigo, nroIdentificacion,
            telefono, fax, email, representante, CopyDirecciones(direcciones));

    public void Actualizar(
        string razonSocial,
        int tipoIdCodigo,
        string nroIdentificacion,
        int longitudRequerida,
        string? telefono,
        string? fax,
        string? email,
        string? representante,
        IReadOnlyList<Direccion> direcciones)
    {
        RazonSocial = NormalizeRazonSocial(razonSocial);
        TipoIdCodigo = tipoIdCodigo;
        NroIdentificacion = ValidateNroIdentificacion(nroIdentificacion, longitudRequerida);
        Telefono = telefono;
        Fax = fax;
        Email = email;
        Representante = representante;
        _direcciones.Clear();
        _direcciones.AddRange(CopyDirecciones(direcciones));
    }

    internal void SetCodigo(int codigo) => Codigo = codigo;

    private static string NormalizeRazonSocial(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("CAT-CLI-001", "Cliente razon social is required.");

        return value.Trim();
    }

    private static string ValidateNroIdentificacion(string value, int longitudRequerida)
    {
        if (value.Length != longitudRequerida)
            throw new DomainException("CAT-CLI-002", "NroIdentificacion length does not match TipoID.");

        return value;
    }

    private static IReadOnlyList<Direccion> CopyDirecciones(IReadOnlyList<Direccion> direcciones) => direcciones.ToList();
}
