using EVANS.Application.Common;
using EVANS.Application.Comprobante.Commands;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.GuiaRemision.Ports;
using MediatR;

namespace EVANS.Application.Comprobante.Handlers;

public sealed class MarcarImpresoCommandHandler : IRequestHandler<MarcarImpresoCommand, Result<bool>>
{
    private readonly IComprobanteRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;

    public MarcarImpresoCommandHandler(IComprobanteRepository repo, IUnitOfWorkFactory uowFactory)
    {
        _repo = repo;
        _uowFactory = uowFactory;
    }

    public Task<Result<bool>> Handle(MarcarImpresoCommand request, CancellationToken cancellationToken)
    {
        using var uow = _uowFactory.Create(request.Year);
        _repo.MarcarImpreso(request.Codigo, uow);
        uow.Commit();

        return Task.FromResult(Result<bool>.Ok(true));
    }
}
