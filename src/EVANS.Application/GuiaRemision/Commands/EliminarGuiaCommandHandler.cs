using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using MediatR;

namespace EVANS.Application.GuiaRemision.Commands;

public sealed class EliminarGuiaCommandHandler : IRequestHandler<EliminarGuiaCommand, Result<bool>>
{
    private readonly IGuiaRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;

    public EliminarGuiaCommandHandler(IGuiaRepository repo, IUnitOfWorkFactory uowFactory)
    {
        _repo = repo;
        _uowFactory = uowFactory;
    }

    public Task<Result<bool>> Handle(EliminarGuiaCommand request, CancellationToken cancellationToken)
    {
        using var uow = _uowFactory.Create(request.Year);
        _repo.Eliminar(request.Codigo, uow);
        uow.Commit();

        return Task.FromResult(Result<bool>.Ok(true));
    }
}
