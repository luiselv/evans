namespace EVANS.Domain.Catalogo;

public sealed class Estado
{
    private Estado(int codigo, string descripcion)
    {
        Codigo = codigo;
        Descripcion = string.IsNullOrWhiteSpace(descripcion)
            ? throw new DomainException("CAT-EST-001", "Estado descripcion is required.")
            : descripcion.Trim();
    }

    public int Codigo { get; private set; }
    public string Descripcion { get; private set; }

    public static Estado Crear(string descripcion) => new(0, descripcion);
    public static Estado Materializar(int codigo, string descripcion) => new(codigo, descripcion);
}
