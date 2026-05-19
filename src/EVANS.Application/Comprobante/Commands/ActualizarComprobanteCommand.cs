using EVANS.Application.Common;
using EVANS.Application.Comprobante.DTOs;
using EVANS.Domain.Comprobante;
using MediatR;

namespace EVANS.Application.Comprobante.Commands;

public record ActualizarComprobanteCommand(
    int Codigo,
    /// <summary>
    /// Serie as stored in the DB. Used by the handler to enforce NumeroComprobante immutability.
    /// Must match the existing DB value or the handler throws DomainException(NUMERO_INMUTABLE).
    /// </summary>
    string Serie,
    /// <summary>
    /// Numero as stored in the DB. Used by the handler to enforce NumeroComprobante immutability.
    /// </summary>
    string Numero,
    TipoComprobante Tipo,
    int ClienteCodigo,
    string RucODni,
    string Direccion,
    IReadOnlyList<DetalleComprobanteInput> Detalles,
    int Year) : IRequest<Result<bool>>;
