namespace EVANS.Domain.Recepcion;

/// <summary>
/// Child entity of Recepcion aggregate.
/// No database PK — delete-and-reinsert is the only safe update strategy (ADR-3).
/// </summary>
public sealed class DetalleRecepcion
{
    public decimal Cantidad { get; private set; }
    public string Descripcion { get; private set; } = string.Empty;
    public decimal Peso { get; private set; }
    public string Unidad { get; private set; } = string.Empty;
    public decimal Costo { get; private set; }
    public string TipoDoc { get; private set; } = string.Empty;
    public string NroDoc { get; private set; } = string.Empty;

    private DetalleRecepcion() { }

    /// <summary>
    /// Factory that enforces all child invariants (DRE001-DRE004).
    /// </summary>
    public static DetalleRecepcion Crear(
        decimal cantidad,
        string descripcion,
        decimal peso,
        string unidad,
        decimal costo,
        string tipoDoc,
        string nroDoc)
    {
        if (cantidad <= 0m)
            throw new DomainException("DRE001", "Cantidad invalida");

        if (string.IsNullOrWhiteSpace(descripcion))
            throw new DomainException("DRE002", "Descripcion requerida");

        if (costo < 0m)
            throw new DomainException("DRE003", "Costo invalido");

        if (peso < 0m)
            throw new DomainException("DRE004", "Peso invalido");

        return new DetalleRecepcion
        {
            Cantidad = cantidad,
            Descripcion = descripcion,
            Peso = peso,
            Unidad = unidad ?? string.Empty,
            Costo = costo,
            TipoDoc = tipoDoc ?? string.Empty,
            NroDoc = nroDoc ?? string.Empty,
        };
    }

    /// <summary>
    /// Computes the cost excluding IGV: Costo / (1 + tasaIgv).
    /// IGV01: tasaIgv must be &gt;= 0.
    /// </summary>
    public decimal CalcularCostoSinIgv(decimal tasaIgv)
    {
        if (tasaIgv < 0m)
            throw new DomainException("IGV01", "Tasa IGV invalida");

        return Math.Round(Costo / (1m + tasaIgv), 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Mutates Costo in-place to its IGV-excluded value.
    /// Called by Recepcion.AplicarIgvEnDetalles.
    /// </summary>
    internal void AjustarCostoSinIgv(decimal tasaIgv) => Costo = CalcularCostoSinIgv(tasaIgv);
}
