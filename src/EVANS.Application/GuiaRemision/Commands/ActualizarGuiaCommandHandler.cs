using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.GuiaRemision.Validators;
using EVANS.Domain.GuiaRemision;
using EVANS.Domain.Shared;
using FluentValidation;
using MediatR;

namespace EVANS.Application.GuiaRemision.Commands;

public sealed class ActualizarGuiaCommandHandler : IRequestHandler<ActualizarGuiaCommand, Result<bool>>
{
    private readonly IGuiaRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly ActualizarGuiaCommandValidator _validator;

    public ActualizarGuiaCommandHandler(IGuiaRepository repo, IUnitOfWorkFactory uowFactory)
    {
        _repo = repo;
        _uowFactory = uowFactory;
        _validator = new ActualizarGuiaCommandValidator();
    }

    public Task<Result<bool>> Handle(ActualizarGuiaCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Validation
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // Step 2: Rebuild aggregate from command
        var detalles = request.Detalles.Select(d => new DetalleGuia(
            null,
            d.Descripcion,
            d.Peso,
            d.PrecioUnitario,
            d.PrecioTotal,
            d.Cantidad));

        var guia = new Guia(
            request.Codigo,
            new NumeroGuia("GREM", 0), // numero preserved from DB — handler doesn't change it
            DateTime.Today,
            request.RemitenteId,
            request.DestinatarioId,
            Direccion.Parse(request.DireccionPartida),
            Direccion.Parse(request.DireccionLlegada),
            request.HasManifest,
            request.VehiculoId,
            request.CarretaId,
            request.ChoferId,
            request.Igv,
            detalles);

        // Step 3: Persist in yearly-DB Serializable transaction
        using var uow = _uowFactory.Create(request.Year);
        _repo.Actualizar(guia, uow);
        uow.Commit();

        return Task.FromResult(Result<bool>.Ok(true));
    }
}
