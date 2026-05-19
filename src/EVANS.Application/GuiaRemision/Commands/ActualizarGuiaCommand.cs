using EVANS.Application.Common;
using MediatR;

namespace EVANS.Application.GuiaRemision.Commands;

public record ActualizarGuiaCommand(
    int Codigo,
    int RemitenteId,
    int DestinatarioId,
    string DireccionPartida,
    string DireccionLlegada,
    bool HasManifest,
    int? VehiculoId,
    int? CarretaId,
    int? ChoferId,
    decimal Igv,
    int Year,
    IReadOnlyList<DetalleGuiaInput> Detalles) : IRequest<Result<bool>>;
