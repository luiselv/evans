namespace EVANS.Domain.Catalogo;

public sealed class Destino
{
    private Destino(int codigo, string descripcion, double distanciaVirtual, int estadoCodigo)
    {
        Codigo = codigo;
        Descripcion = RequireText(descripcion, "CAT-DES-001", "Descripcion is required.");
        if (distanciaVirtual < 0)
            throw new DomainException("CAT-DES-002", "Distancia virtual must be non-negative.");

        DistanciaVirtual = distanciaVirtual;
        EstadoCodigo = estadoCodigo;
    }

    public int Codigo { get; private set; }
    public string Descripcion { get; private set; }
    public double DistanciaVirtual { get; private set; }
    public int EstadoCodigo { get; private set; }

    public static Destino Crear(string descripcion, double distanciaVirtual) =>
        new(0, descripcion, distanciaVirtual, CatalogoEstado.Activo);

    public static Destino Crear(string descripcion, double distanciaVirtual, int estadoCodigo) =>
        new(0, descripcion, distanciaVirtual, estadoCodigo);

    public static Destino Materializar(int codigo, string descripcion, double distanciaVirtual, int estadoCodigo) =>
        new(codigo, descripcion, distanciaVirtual, estadoCodigo);

    public void Deactivate() => EstadoCodigo = CatalogoEstado.Inactivo;

    private static string RequireText(string value, string code, string message) =>
        string.IsNullOrWhiteSpace(value) ? throw new DomainException(code, message) : value.Trim();
}
