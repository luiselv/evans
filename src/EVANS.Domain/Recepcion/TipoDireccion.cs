namespace EVANS.Domain.Recepcion;

/// <summary>
/// Represents the type of address for origin and destination in a Recepcion.
/// Maps to the integer values stored in RECE_TIPODIRPARTIDA / RECE_TIPODIRDESTINO columns.
/// </summary>
public enum TipoDireccion
{
    Agencia         = 0,
    DireccionCliente = 1
}
