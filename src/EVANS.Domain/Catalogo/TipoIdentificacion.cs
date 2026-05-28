namespace EVANS.Domain.Catalogo;

public sealed class TipoIdentificacion
{
    private TipoIdentificacion(int codigo, string descripcion)
    {
        Codigo = codigo;
        Descripcion = string.IsNullOrWhiteSpace(descripcion)
            ? throw new DomainException("CAT-TID-001", "Tipo identificacion descripcion is required.")
            : descripcion.Trim();
        LongitudRequerida = codigo switch
        {
            1 => 11,
            2 => 8,
            _ => throw new DomainException("CAT-TID-002", "Tipo identificacion codigo must be RUC(1) or DNI(2).")
        };
    }

    public int Codigo { get; private set; }
    public string Descripcion { get; private set; }
    public int LongitudRequerida { get; }

    public static TipoIdentificacion Materializar(int codigo, string descripcion) => new(codigo, descripcion);
}
