namespace EVANS.Domain.Manifiesto;

public class Manifiesto
{
    public int Codigo { get; private set; }
    public NumeroManifiesto? Numero { get; private set; }
    public DateTime Fecha { get; }
    public int TransportistaCodigo { get; }
    public int VehiculoCodigo { get; }
    public int? CarretaCodigo { get; }
    public int ChoferCodigo { get; }
    public decimal Importe { get; }
    public decimal Peso { get; }
    public int EstadoCodigo { get; }
    public int UsuarioCodigo { get; }
    public IReadOnlyList<int> GuiaIds { get; }

    private Manifiesto(
        DateTime fecha,
        int transportistaCodigo,
        int vehiculoCodigo,
        int? carretaCodigo,
        int choferCodigo,
        decimal importe,
        decimal peso,
        int estadoCodigo,
        int usuarioCodigo,
        IReadOnlyList<int> guiaIds)
    {
        Fecha = fecha;
        TransportistaCodigo = transportistaCodigo;
        VehiculoCodigo = vehiculoCodigo;
        CarretaCodigo = carretaCodigo;
        ChoferCodigo = choferCodigo;
        Importe = importe;
        Peso = peso;
        EstadoCodigo = estadoCodigo;
        UsuarioCodigo = usuarioCodigo;
        GuiaIds = guiaIds;
    }

    public static Manifiesto Crear(
        DateTime fecha,
        int transportistaCodigo,
        int vehiculoCodigo,
        int? carretaCodigo,
        int choferCodigo,
        decimal importe,
        decimal peso,
        int estadoCodigo,
        int usuarioCodigo,
        IReadOnlyList<int> guiaIds)
    {
        if (guiaIds == null || guiaIds.Count == 0)
            throw new DomainException("MANIFIESTO_SIN_GUIAS", "Se requiere al menos una guía.");

        if (transportistaCodigo <= 0)
            throw new DomainException("TRANSPORTISTA_REQUERIDO", "El código de transportista es requerido.");

        if (vehiculoCodigo <= 0)
            throw new DomainException("VEHICULO_REQUERIDO", "El código de vehículo es requerido.");

        if (choferCodigo <= 0)
            throw new DomainException("CHOFER_REQUERIDO", "El código de chofer es requerido.");

        if (estadoCodigo <= 0)
            throw new DomainException("ESTADO_REQUERIDO", "El código de estado es requerido.");

        if (importe < 0)
            throw new DomainException("IMPORTE_INVALIDO", "El importe no puede ser negativo.");

        return new Manifiesto(fecha, transportistaCodigo, vehiculoCodigo, carretaCodigo,
            choferCodigo, importe, peso, estadoCodigo, usuarioCodigo, guiaIds);
    }

    /// <summary>Sets Codigo after INSERT (called by repository).</summary>
    public void SetCodigo(int codigo)
    {
        Codigo = codigo;
    }

    /// <summary>Sets Numero after numerador assignment.</summary>
    public void SetNumero(NumeroManifiesto numero)
    {
        Numero = numero;
    }
}
