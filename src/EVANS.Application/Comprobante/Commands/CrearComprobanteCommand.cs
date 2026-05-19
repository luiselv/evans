using EVANS.Application.Common;
using EVANS.Application.Comprobante.DTOs;
using EVANS.Domain.Comprobante;
using MediatR;

namespace EVANS.Application.Comprobante.Commands;

public record CrearComprobanteCommand(
    TipoComprobante Tipo,
    int ClienteCodigo,
    string RucODni,
    string Direccion,
    IReadOnlyList<DetalleComprobanteInput> Detalles,
    string? GuiaRef,    // null = Standalone
    int Year) : IRequest<Result<int?>>;
