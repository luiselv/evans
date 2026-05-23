using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Recepcion.Ports;
using EVANS.Domain.Recepcion;
using FluentValidation;
using MediatR;
using Agg = EVANS.Domain.Recepcion.Recepcion;

namespace EVANS.Application.Recepcion.Commands;

public sealed class CrearRecepcionCommandHandler : IRequestHandler<CrearRecepcionCommand, Result<int>>
{
    private readonly IRecepcionRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly CrearRecepcionCommandValidator _validator;

    public CrearRecepcionCommandHandler(IRecepcionRepository repo, IUnitOfWorkFactory uowFactory)
    {
        _repo = repo;
        _uowFactory = uowFactory;
        _validator = new CrearRecepcionCommandValidator();
    }

    public async Task<Result<int>> Handle(CrearRecepcionCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Validate
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // Step 2: Build child entities
        var detalles = request.Detalles
            .Select(d => DetalleRecepcion.Crear(
                d.Cantidad, d.Descripcion, d.Peso, d.Unidad, d.Costo, d.TipoDoc, d.NroDoc))
            .ToList()
            .AsReadOnly();

        // Step 3: Build aggregate
        var recepcion = Agg.Crear(
            request.FechaEmision,
            request.RemitenteId, request.TipoDirPartida, request.DireccionPartida,
            request.DestinatarioId, request.TipoDirDestino, request.DireccionDestino,
            request.DestinoId, request.EstadoId,
            request.Bultos, request.PesoTotal, request.CostoTotal,
            request.Observacion, request.UsuarioId, detalles);

        // Step 4: Apply IGV if requested
        if (request.AplicarIgv)
            recepcion.AplicarIgvEnDetalles(request.TasaIgv);

        // Step 5: Persist
        using var uow = _uowFactory.Create(request.Year);
        var codigo = await _repo.CrearAsync(recepcion, uow, cancellationToken);
        uow.Commit();

        return Result<int>.Ok(codigo);
    }
}
