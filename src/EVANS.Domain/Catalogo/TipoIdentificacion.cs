namespace EVANS.Domain.Catalogo;

public sealed class TipoIdentificacion
{
    private TipoIdentificacion(int codigo, string descripcion, bool allowTransient = false)
    {
        if (codigo <= 0 && !allowTransient)
            throw new DomainException("CAT-TID-002", "Tipo identificacion codigo is required.");

        Codigo = codigo;
        Descripcion = string.IsNullOrWhiteSpace(descripcion)
            ? throw new DomainException("CAT-TID-001", "Tipo identificacion descripcion is required.")
            : descripcion.Trim();
        LongitudRequerida = codigo switch
        {
            1 => 11,
            2 => 8,
            _ => 0
        };
    }

    public int Codigo { get; private set; }
    public string Descripcion { get; private set; }
    public int LongitudRequerida { get; }

    public static TipoIdentificacion Crear(string descripcion) => new(0, descripcion, allowTransient: true);

    public static TipoIdentificacion Materializar(int codigo, string descripcion) => new(codigo, descripcion);
}
