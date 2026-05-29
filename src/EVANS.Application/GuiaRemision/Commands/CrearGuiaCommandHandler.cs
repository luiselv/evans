using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.GuiaRemision.Validators;
using EVANS.Domain.GuiaRemision;
using EVANS.Domain.Shared;
using FluentValidation;
using MediatR;

namespace EVANS.Application.GuiaRemision.Commands;

public sealed class CrearGuiaCommandHandler : IRequestHandler<CrearGuiaCommand, Result<int?>>
{
    private readonly INumeradorService _numerador;
    private readonly IGuiaRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IRecepcionVinculadaService _recepcionService;
    private readonly CrearGuiaCommandValidator _validator;

    public CrearGuiaCommandHandler(
        INumeradorService numerador,
        IGuiaRepository repo,
        IUnitOfWorkFactory uowFactory,
        IRecepcionVinculadaService recepcionService)
    {
        _numerador = numerador;
        _repo = repo;
        _uowFactory = uowFactory;
        _recepcionService = recepcionService;
        _validator = new CrearGuiaCommandValidator();
    }

    public Task<Result<int?>> Handle(CrearGuiaCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Validation — fail before any DB touches
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // Step 2: Numerador — master-DB, own connection
        var numero = _numerador.IncrementarYObtenerGuia();

        // Step 3: Build aggregate (constructor invariants)
        var detalles = request.Detalles.Select(d => new DetalleGuia(
            null,
            d.Descripcion,
            d.Peso,
            d.PrecioUnitario,
            d.PrecioTotal,
            d.Cantidad));

        var guia = new Guia(
            null,
            numero,
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

        // Step 4: Persist in yearly-DB Serializable transaction
        using var uow = _uowFactory.Create(request.Year);
        var codigo = _repo.Insertar(guia, uow);
        uow.Commit();

        // Step 5: Post-commit cross-aggregate write — only if DesdeRecepcion
        if (request.Origen is DesdeRecepcion d)
            _recepcionService.VincularRecepcion(d.RecepcionId, guia.Numero, request.Year);

        // Step 6: Return result
        return Task.FromResult(Result<int?>.Ok(codigo));
    }
}
