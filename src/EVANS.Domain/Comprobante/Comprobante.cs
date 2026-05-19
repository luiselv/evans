namespace EVANS.Domain.Comprobante;

public class Comprobante
{
    private bool _impreso;

    // Backing field settable via reflection from repository (post-INSERT pattern)
    public int? Codigo { get; private set; }

    public NumeroComprobante Numero { get; }
    public TipoComprobante Tipo { get; }
    public DateTime Fecha { get; }
    public int ClienteCodigo { get; }
    public string RucODni { get; }
    public string Direccion { get; }
    public decimal Total { get; }
    public decimal IGV { get; }
    public decimal ValorVenta { get; }
    public bool Impreso => _impreso;
    public OrigenComprobante Origen { get; }
    public IReadOnlyList<DetalleComprobante> Detalles { get; }

    private Comprobante(
        NumeroComprobante numero,
        TipoComprobante tipo,
        DateTime fecha,
        int clienteCodigo,
        string rucODni,
        string direccion,
        IReadOnlyList<DetalleComprobante> detalles,
        OrigenComprobante origen,
        decimal total,
        decimal igv,
        decimal valorVenta)
    {
        Numero = numero;
        Tipo = tipo;
        Fecha = fecha;
        ClienteCodigo = clienteCodigo;
        RucODni = rucODni;
        Direccion = direccion;
        Detalles = detalles;
        Origen = origen;
        Total = total;
        IGV = igv;
        ValorVenta = valorVenta;
    }

    public static Comprobante CrearFactura(
        NumeroComprobante numero,
        DateTime fecha,
        int clienteCodigo,
        string ruc,
        string direccion,
        IReadOnlyList<DetalleComprobante> detalles,
        OrigenComprobante origen,
        decimal igvRate)
    {
        if (detalles == null || detalles.Count == 0)
            throw new DomainException("Comprobante must have at least one DetalleComprobante.");

        if (string.IsNullOrWhiteSpace(ruc))
            throw new DomainException("Factura requires a non-empty RUC.");

        if (igvRate <= 0m)
            throw new DomainException("IGV rate must be greater than zero for Factura.");

        var total = detalles.Sum(d => d.Flete);

        if (total <= 0m)
            throw new DomainException("Total must be greater than zero.");

        var igv = Math.Round(total * igvRate / (1 + igvRate), 2, MidpointRounding.AwayFromZero);
        var valorVenta = Math.Round(total - igv, 2, MidpointRounding.AwayFromZero);

        return new Comprobante(numero, TipoComprobante.Factura, fecha, clienteCodigo,
            ruc, direccion, detalles, origen, total, igv, valorVenta);
    }

    public static Comprobante CrearBoleta(
        NumeroComprobante numero,
        DateTime fecha,
        int clienteCodigo,
        string dni,
        string direccion,
        IReadOnlyList<DetalleComprobante> detalles,
        OrigenComprobante origen)
    {
        if (detalles == null || detalles.Count == 0)
            throw new DomainException("Comprobante must have at least one DetalleComprobante.");

        if (string.IsNullOrWhiteSpace(dni))
            throw new DomainException("Boleta requires a non-empty DNI.");

        var total = detalles.Sum(d => d.Flete);

        if (total <= 0m)
            throw new DomainException("Total must be greater than zero.");

        return new Comprobante(numero, TipoComprobante.Boleta, fecha, clienteCodigo,
            dni, direccion, detalles, origen, total, 0m, 0m);
    }

    public void MarcarImpreso()
    {
        _impreso = true;
    }

    /// <summary>
    /// Sets Codigo after INSERT (called by repository) or before UPDATE (called by handler).
    /// Used to attach the DB-assigned identifier to the aggregate.
    /// </summary>
    public void SetCodigo(int codigo)
    {
        Codigo = codigo;
    }
}
