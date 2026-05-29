using EVANS.Application.Common;
using MediatR;

namespace EVANS.Application.Recepcion.Commands;

public record EliminarRecepcionCommand(int Codigo, int Year) : IRequest<Result<bool>>;
