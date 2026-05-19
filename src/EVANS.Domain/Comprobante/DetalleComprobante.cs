namespace EVANS.Domain.Comprobante;

public class DetalleComprobante
{
    public int Cantidad { get; }
    public string Descripcion { get; }
    public decimal PrecioUnitario { get; }
    public decimal Flete { get; }

    public DetalleComprobante(int cantidad, string descripcion, decimal precioUnitario, decimal flete)
    {
        if (cantidad <= 0)
            throw new ArgumentException("Cantidad must be greater than zero.", nameof(cantidad));

        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("Descripcion cannot be empty.", nameof(descripcion));

        if (flete < 0)
            throw new ArgumentException("Flete cannot be negative.", nameof(flete));

        Cantidad = cantidad;
        Descripcion = descripcion;
        PrecioUnitario = precioUnitario;
        Flete = flete;
    }
}
