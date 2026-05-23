using EVANS.Domain.Recepcion;

namespace EVANS.Application.Recepcion.DTOs;

/// <summary>Full Recepcion DTO with embedded detail lines — returned by ObtenerPorCodigoAsync.</summary>
public record RecepcionDetalleDto(
    int Codigo,
    DateTime FechaEmision,
    int RemitenteCodigo,
    string RemitenteNombre,
    TipoDireccion TipoDirPartida,
    string DireccionPartida,
    int DestinatarioCodigo,
    string DestinatarioNombre,
    TipoDireccion TipoDirDestino,
    string DireccionDestino,
    int DestinoCodigo,
    string DestinoNombre,
    int EstadoCodigo,
    string EstadoNombre,
    int? Bultos,
    decimal? PesoTotal,
    decimal CostoTotal,
    string? GuiaRemisionVinculada,
    string? Observacion,
    int UsuarioCodigo,
    IReadOnlyList<DetalleRecepcionDto> Detalles);
