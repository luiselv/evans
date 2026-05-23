using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Recepcion.Ports;
using FluentValidation;
using MediatR;

namespace EVANS.Application.Recepcion.Commands;

public sealed class EliminarRecepcionCommandHandler : IRequestHandler<EliminarRecepcionCommand, Result<bool>>
{
    private readonly IRecepcionRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly EliminarRecepcionCommandValidator _validator;

    public EliminarRecepcionCommandHandler(IRecepcionRepository repo, IUnitOfWorkFactory uowFactory)
    {
        _repo = repo;
        _uowFactory = uowFactory;
        _validator = new EliminarRecepcionCommandValidator();
    }

    public async Task<Result<bool>> Handle(EliminarRecepcionCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Validate
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // Step 2: Delete (cascades DetalleRecepcion) — NO check on GuiaRemisionVinculada (UI responsibility)
        using var uow = _uowFactory.Create(request.Year);
        await _repo.EliminarAsync(request.Codigo, uow, cancellationToken);
        uow.Commit();

        return Result<bool>.Ok(true);
    }
}
