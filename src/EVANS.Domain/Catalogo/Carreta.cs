namespace EVANS.Domain.Catalogo;

public sealed class Carreta
{
    private Carreta(int codigo, string placa, string? marca, string? certificado, int empresaCodigo, int estadoCodigo)
    {
        Codigo = codigo;
        Placa = RequireText(placa, "CAT-CAR-001", "Placa is required.").ToUpperInvariant();
        Marca = marca;
        Certificado = certificado;
        EmpresaCodigo = empresaCodigo;
        EstadoCodigo = estadoCodigo;
    }

    public int Codigo { get; private set; }
    public string Placa { get; private set; }
    public string? Marca { get; private set; }
    public string? Certificado { get; private set; }
    public int EmpresaCodigo { get; private set; }
    public int EstadoCodigo { get; private set; }

    public static Carreta Crear(
        string placa,
        string? marca,
        string? certificado,
        int empresaCodigo,
        int estadoCodigo = CatalogoEstado.Activo) =>
        new(0, placa, marca, certificado, empresaCodigo, estadoCodigo);

    public static Carreta Materializar(int codigo, string placa, string? marca, string? certificado, int empresaCodigo, int estadoCodigo) =>
        new(codigo, placa, marca, certificado, empresaCodigo, estadoCodigo);

    public void Deactivate() => EstadoCodigo = CatalogoEstado.Inactivo;

    private static string RequireText(string value, string code, string message) =>
        string.IsNullOrWhiteSpace(value) ? throw new DomainException(code, message) : value.Trim();
}
