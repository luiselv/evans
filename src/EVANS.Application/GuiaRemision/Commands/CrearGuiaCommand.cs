using EVANS.Application.Common;
using EVANS.Domain.GuiaRemision;
using MediatR;

namespace EVANS.Application.GuiaRemision.Commands;

public record CrearGuiaCommand(
    int RemitenteId,
    int DestinatarioId,
    string DireccionPartida,
    string DireccionLlegada,
    bool HasManifest,
    int? VehiculoId,
    int? CarretaId,
    int? ChoferId,
    decimal Igv,
    OrigenGuia Origen,
    int Year,
    IReadOnlyList<DetalleGuiaInput> Detalles) : IRequest<Result<int?>>;
