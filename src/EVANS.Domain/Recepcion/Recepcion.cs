namespace EVANS.Domain.Recepcion;

/// <summary>
/// Aggregate root for the Recepcion bounded context.
/// Private constructor — use Crear() factory or Materializar() for repo hydration.
/// </summary>
public sealed class Recepcion
{
    private readonly List<DetalleRecepcion> _detalles = new();

    public int Codigo { get; private set; }           // 0 until SetCodigo() post-INSERT
    public DateTime FechaEmision { get; private set; }
    public int RemitenteId { get; private set; }
    public TipoDireccion TipoDirPartida { get; private set; }
    public string DireccionPartida { get; private set; } = string.Empty;
    public int DestinatarioId { get; private set; }
    public TipoDireccion TipoDirDestino { get; private set; }
    public string DireccionDestino { get; private set; } = string.Empty;
    public int DestinoId { get; private set; }
    public int EstadoId { get; private set; }
    public int? Bultos { get; private set; }
    public decimal? PesoTotal { get; private set; }
    public decimal CostoTotal { get; private set; }
    public string? GuiaRemisionVinculada { get; private set; }  // written only by IRecepcionVinculadaService
    public string? Observacion { get; private set; }
    public int UsuarioId { get; private set; }

    public IReadOnlyList<DetalleRecepcion> Detalles => _detalles.AsReadOnly();

    private Recepcion() { }

    // ------------------------------------------------------------------
    // Factory (enforces all invariants — REC001-REC007)
    // ------------------------------------------------------------------

    public static Recepcion Crear(
        DateTime fechaEmision,
        int remitenteId, TipoDireccion tipoDirPartida, string direccionPartida,
        int destinatarioId, TipoDireccion tipoDirDestino, string direccionDestino,
        int destinoId, int estadoId,
        int? bultos, decimal? pesoTotal, decimal costoTotal,
        string? observacion, int usuarioId,
        IReadOnlyList<DetalleRecepcion> detalles)
    {
        if (fechaEmision == default)
            throw new DomainException("REC006", "Fecha de emision requerida");
        if (remitenteId <= 0)
            throw new DomainException("REC001", "Remitente requerido");
        if (destinatarioId <= 0)
            throw new DomainException("REC002", "Destinatario requerido");
        if (destinoId <= 0)
            throw new DomainException("REC003", "Destino requerido");
        if (estadoId <= 0)
            throw new DomainException("REC004", "Estado requerido");
        if (usuarioId <= 0)
            throw new DomainException("REC007", "Usuario requerido");
        if (detalles is null || detalles.Count == 0)
            throw new DomainException("REC005", "La recepcion debe tener al menos un detalle");

        var r = new Recepcion
        {
            FechaEmision = fechaEmision,
            RemitenteId = remitenteId, TipoDirPartida = tipoDirPartida,
            DireccionPartida = direccionPartida ?? string.Empty,
            DestinatarioId = destinatarioId, TipoDirDestino = tipoDirDestino,
            DireccionDestino = direccionDestino ?? string.Empty,
            DestinoId = destinoId, EstadoId = estadoId,
            Bultos = bultos, PesoTotal = pesoTotal, CostoTotal = costoTotal,
            Observacion = observacion, UsuarioId = usuarioId,
        };
        r._detalles.AddRange(detalles);
        return r;
    }

    // ------------------------------------------------------------------
    // Materializer (bypasses invariant checks — for repository hydration only)
    // ------------------------------------------------------------------

    public static Recepcion Materializar(
        int codigo, DateTime fechaEmision,
        int remitenteId, TipoDireccion tipoDirPartida, string direccionPartida,
        int destinatarioId, TipoDireccion tipoDirDestino, string direccionDestino,
        int destinoId, int estadoId,
        int? bultos, decimal? pesoTotal, decimal costoTotal,
        string? guiaRemisionVinculada, string? observacion, int usuarioId,
        IReadOnlyList<DetalleRecepcion> detalles)
    {
        var r = new Recepcion
        {
            Codigo = codigo,
            FechaEmision = fechaEmision,
            RemitenteId = remitenteId, TipoDirPartida = tipoDirPartida,
            DireccionPartida = direccionPartida ?? string.Empty,
            DestinatarioId = destinatarioId, TipoDirDestino = tipoDirDestino,
            DireccionDestino = direccionDestino ?? string.Empty,
            DestinoId = destinoId, EstadoId = estadoId,
            Bultos = bultos, PesoTotal = pesoTotal, CostoTotal = costoTotal,
            GuiaRemisionVinculada = guiaRemisionVinculada,
            Observacion = observacion, UsuarioId = usuarioId,
        };
        r._detalles.AddRange(detalles);
        return r;
    }

    // ------------------------------------------------------------------
    // Mutations
    // ------------------------------------------------------------------

    /// <summary>
    /// Replaces all header fields and the detail collection entirely.
    /// Enforces same invariants as Crear() (excluding UsuarioId which is immutable).
    /// </summary>
    public void Actualizar(
        DateTime fechaEmision,
        int remitenteId, TipoDireccion tipoDirPartida, string direccionPartida,
        int destinatarioId, TipoDireccion tipoDirDestino, string direccionDestino,
        int destinoId, int estadoId,
        int? bultos, decimal? pesoTotal, decimal costoTotal,
        string? observacion,
        IReadOnlyList<DetalleRecepcion> nuevosDetalles)
    {
        if (Codigo <= 0)
            throw new InvalidOperationException("Cannot Actualizar a non-persisted Recepcion.");

        if (fechaEmision == default)
            throw new DomainException("REC006", "Fecha de emision requerida");
        if (remitenteId <= 0)
            throw new DomainException("REC001", "Remitente requerido");
        if (destinatarioId <= 0)
            throw new DomainException("REC002", "Destinatario requerido");
        if (destinoId <= 0)
            throw new DomainException("REC003", "Destino requerido");
        if (estadoId <= 0)
            throw new DomainException("REC004", "Estado requerido");
        if (nuevosDetalles is null || nuevosDetalles.Count == 0)
            throw new DomainException("REC005", "La recepcion debe tener al menos un detalle");

        FechaEmision = fechaEmision;
        RemitenteId = remitenteId; TipoDirPartida = tipoDirPartida;
        DireccionPartida = direccionPartida ?? string.Empty;
        DestinatarioId = destinatarioId; TipoDirDestino = tipoDirDestino;
        DireccionDestino = direccionDestino ?? string.Empty;
        DestinoId = destinoId; EstadoId = estadoId;
        Bultos = bultos; PesoTotal = pesoTotal; CostoTotal = costoTotal;
        Observacion = observacion;

        _detalles.Clear();
        _detalles.AddRange(nuevosDetalles);
    }

    /// <summary>Set by repository post-INSERT. May only be called once.</summary>
    internal void SetCodigo(int codigo)
    {
        if (Codigo != 0) throw new InvalidOperationException("Codigo already set.");
        if (codigo <= 0) throw new ArgumentOutOfRangeException(nameof(codigo));
        Codigo = codigo;
    }

    /// <summary>Set by repository on materialization — RECE_GUIAREMISION read-back only.</summary>
    internal void SetGuiaRemisionVinculada(string? numero) => GuiaRemisionVinculada = numero;

    /// <summary>
    /// Applies IGV exclusion to all detail costs.
    /// IGV01: tasaIgv must be &gt;= 0.
    /// Called by handlers when cmd.AplicarIgv == true.
    /// </summary>
    public void AplicarIgvEnDetalles(decimal tasaIgv)
    {
        if (tasaIgv < 0m)
            throw new DomainException("IGV01", "Tasa IGV invalida");

        foreach (var d in _detalles)
            d.AjustarCostoSinIgv(tasaIgv);
    }
}
