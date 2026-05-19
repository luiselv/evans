using EVANS.Application.Common;
using MediatR;

namespace EVANS.Application.Comprobante.Commands;

public record EliminarComprobanteCommand(int Codigo, int Year) : IRequest<Result<bool>>;
