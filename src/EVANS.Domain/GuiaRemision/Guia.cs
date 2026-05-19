namespace EVANS.Domain.GuiaRemision;

public class Guia
{
    private List<DetalleGuia> _detalles;

    public int? Codigo { get; }
    public NumeroGuia Numero { get; }
    public DateTime Fecha { get; }
    public int RemitenteId { get; }
    public int DestinatarioId { get; }
    public Direccion DireccionPartida { get; }
    public Direccion DireccionLlegada { get; }
    public bool HasManifest { get; }
    public int? VehiculoId { get; }
    public int? CarretaId { get; }
    public int? ChoferId { get; }
    public decimal Igv { get; }
    public IReadOnlyList<DetalleGuia> Detalles => _detalles.AsReadOnly();

    public Guia(
        int? codigo,
        NumeroGuia numero,
        DateTime fecha,
        int remitenteId,
        int destinatarioId,
        Direccion direccionPartida,
        Direccion direccionLlegada,
        bool hasManifest,
        int? vehiculoId,
        int? carretaId,
        int? choferId,
        decimal igv,
        IEnumerable<DetalleGuia> detalles)
    {
        var detallesList = detalles?.ToList() ?? new List<DetalleGuia>();

        if (detallesList.Count == 0)
            throw new DomainException("Guia must have at least one DetalleGuia.");

        if (remitenteId <= 0)
            throw new DomainException("RemitenteId must be greater than zero.");

        if (destinatarioId <= 0)
            throw new DomainException("DestinatarioId must be greater than zero.");

        if (hasManifest && (vehiculoId is null || carretaId is null || choferId is null))
            throw new DomainException("When HasManifest is true, VehiculoId, CarretaId, and ChoferId must all be set.");

        Codigo = codigo;
        Numero = numero;
        Fecha = fecha;
        RemitenteId = remitenteId;
        DestinatarioId = destinatarioId;
        DireccionPartida = direccionPartida;
        DireccionLlegada = direccionLlegada;
        HasManifest = hasManifest;
        VehiculoId = vehiculoId;
        CarretaId = carretaId;
        ChoferId = choferId;
        Igv = igv;
        _detalles = detallesList;
    }

    public void ReemplazarDetalles(IEnumerable<DetalleGuia> newDetalles)
    {
        var list = newDetalles?.ToList() ?? new List<DetalleGuia>();
        if (list.Count == 0)
            throw new DomainException("Guia must have at least one DetalleGuia.");

        _detalles = list;
    }

    public decimal CalcularSubtotal() =>
        _detalles.Sum(d => d.PrecioTotal / (1 + Igv));

    public decimal CalcularIgv(decimal igvRate) =>
        _detalles.Sum(d => d.PrecioTotal - d.PrecioTotal / (1 + igvRate));
}
