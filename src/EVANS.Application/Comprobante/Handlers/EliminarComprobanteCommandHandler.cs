using EVANS.Application.Common;
using EVANS.Application.Comprobante.Commands;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.GuiaRemision.Ports;
using MediatR;

namespace EVANS.Application.Comprobante.Handlers;

public sealed class EliminarComprobanteCommandHandler : IRequestHandler<EliminarComprobanteCommand, Result<bool>>
{
    private readonly IComprobanteRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;

    public EliminarComprobanteCommandHandler(IComprobanteRepository repo, IUnitOfWorkFactory uowFactory)
    {
        _repo = repo;
        _uowFactory = uowFactory;
    }

    public Task<Result<bool>> Handle(EliminarComprobanteCommand request, CancellationToken cancellationToken)
    {
        using var uow = _uowFactory.Create(request.Year);
        _repo.Eliminar(request.Codigo, uow);
        uow.Commit();

        return Task.FromResult(Result<bool>.Ok(true));
    }
}
