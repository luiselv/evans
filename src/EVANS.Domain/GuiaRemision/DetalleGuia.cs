namespace EVANS.Domain.GuiaRemision;

public class DetalleGuia
{
    public int? Codigo { get; }
    public string Descripcion { get; }
    public Peso Peso { get; }
    public decimal PrecioUnitario { get; }
    public decimal PrecioTotal { get; }
    public int Cantidad { get; }

    public DetalleGuia(
        int? codigo,
        string descripcion,
        Peso peso,
        decimal precioUnitario,
        decimal precioTotal,
        int cantidad)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("Descripcion cannot be empty.", nameof(descripcion));

        if (cantidad <= 0)
            throw new ArgumentException("Cantidad must be greater than zero.", nameof(cantidad));

        if (precioUnitario < 0)
            throw new ArgumentException("PrecioUnitario cannot be negative.", nameof(precioUnitario));

        if (precioTotal < 0)
            throw new ArgumentException("PrecioTotal cannot be negative.", nameof(precioTotal));

        Codigo = codigo;
        Descripcion = descripcion;
        Peso = peso;
        PrecioUnitario = precioUnitario;
        PrecioTotal = precioTotal;
        Cantidad = cantidad;
    }
}
