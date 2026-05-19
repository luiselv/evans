namespace EVANS.Domain.Comprobante;

public abstract record OrigenComprobante;

public sealed record Standalone : OrigenComprobante;

public sealed record DesdeGuia : OrigenComprobante
{
    public string GuiaRef { get; }

    public DesdeGuia(string guiaRef)
    {
        if (string.IsNullOrWhiteSpace(guiaRef))
            throw new ArgumentException("GuiaRef cannot be null or empty.", nameof(guiaRef));

        GuiaRef = guiaRef;
    }
}
