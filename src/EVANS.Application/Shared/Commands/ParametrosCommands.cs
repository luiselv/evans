using EVANS.Application.Common;
using EVANS.Application.Shared.DTOs;
using EVANS.Application.Shared.Ports;
using MediatR;

namespace EVANS.Application.Shared.Commands;

public sealed record ActualizarParametrosCommand(ParametrosDto Parametros) : IRequest<Result<bool>>;

public sealed class ActualizarParametrosCommandHandler(IParametrosService parametrosService)
    : IRequestHandler<ActualizarParametrosCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        ActualizarParametrosCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Parametros.IgvRate <= 0)
            return Result<bool>.Fail("IGV rate is required.");

        await parametrosService.ActualizarParametrosAsync(request.Parametros, cancellationToken);
        return Result<bool>.Ok(true);
    }
}
