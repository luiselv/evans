using EVANS.Application.Common;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Manifiesto.Commands;
using EVANS.Application.Manifiesto.Ports;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EVANS.Application.Manifiesto.Handlers;

public sealed class EliminarManifiestoCommandHandler : IRequestHandler<EliminarManifiestoCommand, Result<bool>>
{
    private readonly IManifiestoRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IGuiasManifiestoService _guiasService;
    private readonly ILogger<EliminarManifiestoCommandHandler>? _logger;

    public EliminarManifiestoCommandHandler(
        IManifiestoRepository repo,
        IUnitOfWorkFactory uowFactory,
        IGuiasManifiestoService guiasService,
        ILogger<EliminarManifiestoCommandHandler>? logger = null)
    {
        _repo = repo;
        _uowFactory = uowFactory;
        _guiasService = guiasService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(EliminarManifiestoCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Load existing to get guia list for post-commit cleanup
        var existing = await _repo.ObtenerPorCodigoAsync(request.Codigo, request.Year, cancellationToken);
        if (existing is null)
            return Result<bool>.Fail($"Manifiesto with Codigo={request.Codigo} not found.");

        var guiaIds = existing.Lineas.Select(l => l.GuiaId).ToList();

        // Step 2: Delete (cascades DetalleManifiesto) in yearly-DB transaction
        using var uow = _uowFactory.Create(request.Year);
        await _repo.EliminarAsync(request.Codigo, uow, cancellationToken);
        uow.Commit();

        // Step 3: Post-commit — mark guias as available (best-effort)
        // NOTE: GREM_MANIFIESTO is NOT reset (legacy behavior preserved per spec SC-4)
        if (guiaIds.Count > 0)
        {
            try
            {
                await _guiasService.MarcarGuiasDisponiblesAsync(guiaIds, request.Year, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex,
                    "MarcarGuiasDisponibles failed for ManifestoCodigo={Codigo} — continuing (best-effort).",
                    request.Codigo);
            }
        }

        return Result<bool>.Ok(true);
    }
}
