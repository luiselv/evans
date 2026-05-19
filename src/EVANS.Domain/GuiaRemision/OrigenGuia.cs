namespace EVANS.Domain.GuiaRemision;

public abstract record OrigenGuia;

public sealed record Standalone : OrigenGuia;

public sealed record DesdeRecepcion(int RecepcionId) : OrigenGuia;
