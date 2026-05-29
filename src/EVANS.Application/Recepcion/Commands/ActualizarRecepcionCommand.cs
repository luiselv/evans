using EVANS.Application.Common;
using EVANS.Domain.Recepcion;
using MediatR;

namespace EVANS.Application.Recepcion.Commands;

public record ActualizarRecepcionCommand(
    int Codigo,
    DateTime FechaEmision,
    int RemitenteId,
    TipoDireccion TipoDirPartida,
    string DireccionPartida,
    int DestinatarioId,
    TipoDireccion TipoDirDestino,
    string DireccionDestino,
    int DestinoId,
    int EstadoId,
    int? Bultos,
    decimal? PesoTotal,
    decimal CostoTotal,
    string? Observacion,
    IReadOnlyList<DetalleRecepcionInput> Detalles,
    int Year,
    bool AplicarIgv = false,
    decimal TasaIgv = 0.18m) : IRequest<Result<bool>>;
