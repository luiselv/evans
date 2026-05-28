namespace EVANS.Domain.Catalogo;

public sealed class Agencia
{
    private Agencia(int codigo, string direccion, int destinoCodigo, int estadoCodigo)
    {
        Codigo = codigo;
        Direccion = string.IsNullOrWhiteSpace(direccion)
            ? throw new DomainException("CAT-AGE-001", "Agencia direccion is required.")
            : direccion.Trim();
        DestinoCodigo = destinoCodigo;
        EstadoCodigo = estadoCodigo;
    }

    public int Codigo { get; private set; }
    public string Direccion { get; private set; }
    public int DestinoCodigo { get; private set; }
    public int EstadoCodigo { get; private set; }

    public static Agencia Materializar(int codigo, string direccion, int destinoCodigo, int estadoCodigo) =>
        new(codigo, direccion, destinoCodigo, estadoCodigo);
}
