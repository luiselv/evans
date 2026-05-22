using EVANS.Application.Common;
using MediatR;

namespace EVANS.Application.Manifiesto.Commands;

public record EliminarManifiestoCommand(int Codigo, int Year) : IRequest<Result<bool>>;
