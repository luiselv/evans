using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Manifiesto.Commands;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Application.Manifiesto.Validators;
using EVANS.Domain.Manifiesto;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Agg = EVANS.Domain.Manifiesto.Manifiesto;

namespace EVANS.Application.Manifiesto.Handlers;

public sealed class CrearManifiestoCommandHandler : IRequestHandler<CrearManifiestoCommand, Result<int?>>
{
    private readonly IManifiestoRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly INumeradorManifiestoService _numerador;
    private readonly IGuiasManifiestoService _guiasService;
    private readonly ILogger<CrearManifiestoCommandHandler>? _logger;
    private readonly CrearManifiestoCommandValidator _validator;

    public CrearManifiestoCommandHandler(
        IManifiestoRepository repo,
        IUnitOfWorkFactory uowFactory,
        INumeradorManifiestoService numerador,
        IGuiasManifiestoService guiasService,
        ILogger<CrearManifiestoCommandHandler>? logger = null)
    {
        _repo = repo;
        _uowFactory = uowFactory;
        _numerador = numerador;
        _guiasService = guiasService;
        _logger = logger;
        _validator = new CrearManifiestoCommandValidator();
    }

    public async Task<Result<int?>> Handle(CrearManifiestoCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Validate — fail before any DB touches
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // Step 2: Numerador — master-DB, own connection (async, before UoW)
        var numero = await _numerador.IncrementarYObtenerAsync(request.Year, cancellationToken);

        // Step 3: Build aggregate (domain invariants enforced in Crear)
        var manifiesto = Agg.Crear(
            fecha: request.Fecha,
            transportistaCodigo: request.TransportistaCodigo,
            vehiculoCodigo: request.VehiculoCodigo,
            carretaCodigo: request.CarretaCodigo,
            choferCodigo: request.ChoferCodigo,
            importe: request.Importe,
            peso: request.Peso,
            estadoCodigo: request.EstadoCodigo,
            usuarioCodigo: request.UsuarioCodigo,
            guiaIds: request.GuiaIds);

        manifiesto.SetNumero(numero);

        // Step 4: Persist in yearly-DB Serializable transaction
        using var uow = _uowFactory.Create(request.Year);
        var codigo = await _repo.InsertarAsync(manifiesto, uow, cancellationToken);
        manifiesto.SetCodigo(codigo);
        uow.Commit();

        // Step 5: Post-commit — mark guias as sent (best-effort)
        try
        {
            var carrier = new CarrierInfo(
                TransportistaCodigo: request.TransportistaCodigo,
                ChoferCodigo: request.ChoferCodigo,
                VehiculoCodigo: request.VehiculoCodigo,
                CarretaCodigo: request.CarretaCodigo);

            var result = await _guiasService.MarcarGuiasEnviadasAsync(
                request.GuiaIds,
                numero.Value,
                request.Fecha,
                carrier,
                request.Year,
                cancellationToken);

            if (result.HasFailures)
            {
                _logger?.LogWarning(
                    "MarcarGuiasEnviadas: {Affected} affected, {NotFoundCount} guias not found: [{NotFound}]",
                    result.Affected, result.NotFound.Count, string.Join(",", result.NotFound));
            }
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex,
                "MarcarGuiasEnviadas failed for ManifiestoNumero={Numero} — continuing (best-effort).",
                numero.Value);
        }

        return Result<int?>.Ok(codigo);
    }
}
