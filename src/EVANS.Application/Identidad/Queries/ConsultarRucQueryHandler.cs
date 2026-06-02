using EVANS.Application.Common;
using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Ports;
using EVANS.Domain.Shared;
using MediatR;

namespace EVANS.Application.Identidad.Queries;

public sealed class ConsultarRucQueryHandler(ISunatRucService sunatRucService)
    : IRequestHandler<ConsultarRucQuery, Result<SunatRucDto>>
{
    public async Task<Result<SunatRucDto>> Handle(
        ConsultarRucQuery request,
        CancellationToken cancellationToken)
    {
        if (!Ruc.TryCreate(request.Ruc, out var ruc))
            return Result<SunatRucDto>.Fail("Invalid RUC.");

        var result = await sunatRucService.ConsultarAsync(ruc.Value, cancellationToken);
        return result is null
            ? Result<SunatRucDto>.Fail("RUC not found.")
            : Result<SunatRucDto>.Ok(result);
    }
}
