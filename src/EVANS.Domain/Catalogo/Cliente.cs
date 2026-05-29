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
        string? email,
        IReadOnlyList<Direccion> direcciones)
    {
        Codigo = codigo;
        RazonSocial = razonSocial;
        TipoIdCodigo = tipoIdCodigo;
        NroIdentificacion = nroIdentificacion;
        Telefono = telefono;
        Email = email;
        _direcciones = direcciones.ToList();
    }

    public int Codigo { get; private set; }
    public string RazonSocial { get; private set; }
    public int TipoIdCodigo { get; private set; }
    public string NroIdentificacion { get; private set; }
    public string? Telefono { get; private set; }
    public string? Email { get; private set; }
    public IReadOnlyList<Direccion> Direcciones => _direcciones.AsReadOnly();

    public static Cliente Crear(
        string razonSocial,
        int tipoIdCodigo,
        string nroIdentificacion,
        int longitudRequerida,
        string? telefono,
        string? email,
        IReadOnlyList<Direccion> direcciones) =>
        new(0, NormalizeRazonSocial(razonSocial), tipoIdCodigo,
            ValidateNroIdentificacion(nroIdentificacion, longitudRequerida),
            telefono, email, ValidateDirecciones(direcciones));

    public static Cliente Materializar(
        int codigo,
        string razonSocial,
        int tipoIdCodigo,
        string nroIdentificacion,
        string? telefono,
        string? email,
        IReadOnlyList<Direccion> direcciones) =>
        new(codigo, NormalizeRazonSocial(razonSocial), tipoIdCodigo, nroIdentificacion,
            telefono, email, ValidateDirecciones(direcciones));

    public void Actualizar(
        string razonSocial,
        int tipoIdCodigo,
        string nroIdentificacion,
        int longitudRequerida,
        string? telefono,
        string? email,
        IReadOnlyList<Direccion> direcciones)
    {
        RazonSocial = NormalizeRazonSocial(razonSocial);
        TipoIdCodigo = tipoIdCodigo;
        NroIdentificacion = ValidateNroIdentificacion(nroIdentificacion, longitudRequerida);
        Telefono = telefono;
        Email = email;
        _direcciones.Clear();
        _direcciones.AddRange(ValidateDirecciones(direcciones));
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

        if (longitudRequerida == 11 && !Ruc.TryCreate(value, out _))
            throw new DomainException("CAT-CLI-002", "RUC must be 11 numeric characters.");

        return value;
    }

    private static IReadOnlyList<Direccion> ValidateDirecciones(IReadOnlyList<Direccion> direcciones)
    {
        if (direcciones.Count == 0)
            throw new DomainException("CAT-CLI-003", "Cliente must have at least one direccion.");

        return direcciones;
    }
}
