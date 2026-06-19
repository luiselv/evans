namespace EVANS.Domain.Catalogo;

public sealed class Vehiculo
{
    private Vehiculo(int codigo, string? marca, string placa, string configuracionVehicular, string? certificadoInscripcion, int empresaCodigo, int estadoCodigo)
    {
        Codigo = codigo;
        Marca = marca;
        Placa = NormalizePlaca(placa, "CAT-VEH-001");
        ConfiguracionVehicular = RequireText(configuracionVehicular, "CAT-VEH-002", "Configuracion vehicular is required.");
        if (ConfiguracionVehicular.Length > 5)
            throw new DomainException("CAT-VEH-003", "Configuracion vehicular cannot exceed 5 characters.");
        CertificadoInscripcion = certificadoInscripcion;
        EmpresaCodigo = empresaCodigo;
        EstadoCodigo = estadoCodigo;
    }

    public int Codigo { get; private set; }
    public string? Marca { get; private set; }
    public string Placa { get; private set; }
    public string ConfiguracionVehicular { get; private set; }
    public string? CertificadoInscripcion { get; private set; }
    public int EmpresaCodigo { get; private set; }
    public int EstadoCodigo { get; private set; }

    public static Vehiculo Crear(
        string? marca,
        string placa,
        string configuracionVehicular,
        string? certificadoInscripcion,
        int empresaCodigo,
        int estadoCodigo = CatalogoEstado.Activo) =>
        new(0, marca, placa, configuracionVehicular, certificadoInscripcion, empresaCodigo, estadoCodigo);

    public static Vehiculo Materializar(int codigo, string? marca, string placa, string configuracionVehicular, string? certificadoInscripcion, int empresaCodigo, int estadoCodigo) =>
        new(codigo, marca, placa, configuracionVehicular, certificadoInscripcion, empresaCodigo, estadoCodigo);

    public void Deactivate() => EstadoCodigo = CatalogoEstado.Inactivo;

    private static string NormalizePlaca(string value, string code) => RequireText(value, code, "Placa is required.").ToUpperInvariant();
    private static string RequireText(string value, string code, string message) =>
        string.IsNullOrWhiteSpace(value) ? throw new DomainException(code, message) : value.Trim();
}
