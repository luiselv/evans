namespace EVANS.Application.GuiaRemision.DTOs;

public record GuiaDetalleDto(
    int Codigo,
    string NumeroGuia,
    DateTime Fecha,
    int RemitenteId,
    string RemitenteNombre,
    int DestinatarioId,
    string DestinatarioNombre,
    string DireccionPartida,
    string DireccionLlegada,
    bool HasManifest,
    int? VehiculoId,
    int? CarretaId,
    int? ChoferId,
    decimal Igv,
    IReadOnlyList<DetalleGuiaItemDto> Detalles);

public record DetalleGuiaItemDto(
    int? Codigo,
    string Descripcion,
    decimal PesoValor,
    decimal PrecioUnitario,
    decimal PrecioTotal,
    int Cantidad);
