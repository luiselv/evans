using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Manifiesto.Commands;
using EVANS.Application.Manifiesto.Ports;
using EVANS.Domain.Manifiesto;
using MediatR;
using Microsoft.Extensions.Logging;
using Agg = EVANS.Domain.Manifiesto.Manifiesto;

namespace EVANS.Application.Manifiesto.Handlers;

public sealed class ActualizarManifiestoCommandHandler : IRequestHandler<ActualizarManifiestoCommand, Result<bool>>
{
    private readonly IManifiestoRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IGuiasManifiestoService _guiasService;
    private readonly ILogger<ActualizarManifiestoCommandHandler>? _logger;

    public ActualizarManifiestoCommandHandler(
        IManifiestoRepository repo,
        IUnitOfWorkFactory uowFactory,
        IGuiasManifiestoService guiasService,
        ILogger<ActualizarManifiestoCommandHandler>? logger = null)
    {
        _repo = repo;
        _uowFactory = uowFactory;
        _guiasService = guiasService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ActualizarManifiestoCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Load existing to get real Codigo and previous guia list (bug-fix: never use Codigo=0)
        var existing = await _repo.ObtenerPorCodigoAsync(request.Codigo, request.Year, cancellationToken);
        if (existing is null)
            return Result<bool>.Fail($"Manifiesto with Codigo={request.Codigo} not found.");

        var previousGuiaIds = existing.Lineas.Select(l => l.GuiaId).ToHashSet();

        // Step 2: Rebuild aggregate from command fields
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

        // Step 3: Set real Codigo so repository UPDATE targets the right record (not 0)
        manifiesto.SetCodigo(request.Codigo);

        // Step 4: Persist in yearly-DB Serializable transaction
        using var uow = _uowFactory.Create(request.Year);
        await _repo.ActualizarAsync(manifiesto, uow, cancellationToken);
        uow.Commit();

        // Step 5: Post-commit — reconcile guia flags (best-effort)
        var newGuiaIds = request.GuiaIds.ToHashSet();
        var removedGuiaIds = previousGuiaIds.Except(newGuiaIds).ToList();
        var addedGuiaIds = newGuiaIds.Except(previousGuiaIds).ToList();

        if (removedGuiaIds.Count > 0)
        {
            try
            {
                await _guiasService.MarcarGuiasDisponiblesAsync(removedGuiaIds, request.Year, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex,
                    "MarcarGuiasDisponibles failed for ManifestoCodigo={Codigo} — continuing (best-effort).",
                    request.Codigo);
            }
        }

        if (addedGuiaIds.Count > 0)
        {
            try
            {
                var carrier = new CarrierInfo(
                    TransportistaCodigo: request.TransportistaCodigo,
                    ChoferCodigo: request.ChoferCodigo,
                    VehiculoCodigo: request.VehiculoCodigo,
                    CarretaCodigo: request.CarretaCodigo);

                var numero = existing.Numero; // preserve existing numero (I-9: immutable once persisted)
                await _guiasService.MarcarGuiasEnviadasAsync(
                    addedGuiaIds, numero, request.Fecha, carrier, request.Year, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex,
                    "MarcarGuiasEnviadas failed for ManifestoCodigo={Codigo} — continuing (best-effort).",
                    request.Codigo);
            }
        }

        return Result<bool>.Ok(true);
    }
}
