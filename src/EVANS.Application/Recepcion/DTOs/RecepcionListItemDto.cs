namespace EVANS.Application.Recepcion.DTOs;

/// <summary>Summary row returned by BuscarPorRangoFechasAsync for grid/list display.</summary>
public record RecepcionListItemDto(
    int Codigo,
    DateTime Fecha,
    int RemitenteCodigo,
    string RemitenteNombre,
    int DestinatarioCodigo,
    string DestinatarioNombre,
    int DestinoCodigo,
    string DestinoNombre,
    int EstadoCodigo,
    string EstadoNombre,
    decimal CostoTotal,
    string? GuiaRemisionVinculada);
