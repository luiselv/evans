using EVANS.Application.Common;
using MediatR;

namespace EVANS.Application.GuiaRemision.Commands;

public record EliminarGuiaCommand(int Codigo, int Year) : IRequest<Result<bool>>;
