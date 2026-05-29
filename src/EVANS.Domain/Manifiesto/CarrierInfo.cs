namespace EVANS.Domain.Manifiesto;

/// <summary>
/// Carrier assignment data used when marking guias as sent.
/// Maps to GuiaRemision columns: EMPR_CODIGO, CHOF_CODIGO, VEHI_CODIGO, CARR_CODIGO.
/// </summary>
public record CarrierInfo(
    int TransportistaCodigo,   // → EMPR_CODIGO on GuiaRemision
    int ChoferCodigo,          // → CHOF_CODIGO on GuiaRemision
    int VehiculoCodigo,        // → VEHI_CODIGO on GuiaRemision
    int? CarretaCodigo);       // → CARR_CODIGO on GuiaRemision (nullable)
