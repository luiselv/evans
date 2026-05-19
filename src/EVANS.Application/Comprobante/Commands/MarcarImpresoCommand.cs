using EVANS.Application.Common;
using MediatR;

namespace EVANS.Application.Comprobante.Commands;

public record MarcarImpresoCommand(int Codigo, int Year) : IRequest<Result<bool>>;
