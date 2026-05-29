using EVANS.Application.Common;
using MediatR;

namespace EVANS.Application.Manifiesto.Commands;

public record ActualizarManifiestoCommand(
    int Codigo,
    DateTime Fecha,
    int TransportistaCodigo,
    int VehiculoCodigo,
    int? CarretaCodigo,
    int ChoferCodigo,
    decimal Importe,
    decimal Peso,
    int EstadoCodigo,
    int UsuarioCodigo,
    IReadOnlyList<int> GuiaIds,
    int Year) : IRequest<Result<bool>>;
