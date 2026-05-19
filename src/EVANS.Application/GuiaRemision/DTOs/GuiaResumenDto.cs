namespace EVANS.Application.GuiaRemision.DTOs;

public record GuiaResumenDto(
    int Codigo,
    string NumeroGuia,
    DateTime Fecha,
    int RemitenteId,
    string RemitenteNombre,
    int DestinatarioId,
    string DestinatarioNombre,
    bool HasManifest,
    decimal Igv);
