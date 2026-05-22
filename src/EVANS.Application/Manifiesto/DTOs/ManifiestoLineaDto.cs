namespace EVANS.Application.Manifiesto.DTOs;

public record ManifiestoLineaDto(
    int GuiaId,
    string NumeroGuia,
    int DestinoCodigo,
    string DestinoNombre,
    decimal Peso,
    decimal Flete);
