using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Recepcion.Ports;
using EVANS.Domain.Recepcion;
using FluentValidation;
using MediatR;
using Agg = EVANS.Domain.Recepcion.Recepcion;

namespace EVANS.Application.Recepcion.Commands;

public sealed class ActualizarRecepcionCommandHandler : IRequestHandler<ActualizarRecepcionCommand, Result<bool>>
{
    private readonly IRecepcionRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly ActualizarRecepcionCommandValidator _validator;

    public ActualizarRecepcionCommandHandler(IRecepcionRepository repo, IUnitOfWorkFactory uowFactory)
    {
        _repo = repo;
        _uowFactory = uowFactory;
        _validator = new ActualizarRecepcionCommandValidator();
    }

    public async Task<Result<bool>> Handle(ActualizarRecepcionCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Validate
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // Step 2: Load existing aggregate
        var existente = await _repo.ObtenerPorCodigoAsync(request.Codigo, request.Year, cancellationToken);
        if (existente is null)
            throw new DomainException("REC404", "Recepcion no encontrada");

        // Step 3: Build new detail list
        var nuevosDetalles = request.Detalles
            .Select(d => DetalleRecepcion.Crear(
                d.Cantidad, d.Descripcion, d.Peso, d.Unidad, d.Costo, d.TipoDoc, d.NroDoc))
            .ToList()
            .AsReadOnly();

        // Step 4: Apply aggregate mutation
        existente.Actualizar(
            request.FechaEmision,
            request.RemitenteId, request.TipoDirPartida, request.DireccionPartida,
            request.DestinatarioId, request.TipoDirDestino, request.DireccionDestino,
            request.DestinoId, request.EstadoId,
            request.Bultos, request.PesoTotal, request.CostoTotal,
            request.Observacion, nuevosDetalles);

        // Step 5: Apply IGV if requested
        if (request.AplicarIgv)
            existente.AplicarIgvEnDetalles(request.TasaIgv);

        // Step 6: Persist (delete-reinsert detalles inside)
        using var uow = _uowFactory.Create(request.Year);
        await _repo.ActualizarAsync(existente, uow, cancellationToken);
        uow.Commit();

        return Result<bool>.Ok(true);
    }
}
