using EVANS.Application.Common;
using EVANS.Application.Comprobante.Commands;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.Comprobante.Validators;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Shared.Ports;
using EVANS.Domain.Comprobante;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Agg = EVANS.Domain.Comprobante.Comprobante;

namespace EVANS.Application.Comprobante.Handlers;

public sealed class CrearComprobanteCommandHandler : IRequestHandler<CrearComprobanteCommand, Result<int?>>
{
    private readonly INumeradorComprobanteService _numerador;
    private readonly IComprobanteRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IGuiaVinculadaService _guiaService;
    private readonly IParametrosService _parametros;
    private readonly CrearComprobanteCommandValidator _validator;
    private readonly ILogger<CrearComprobanteCommandHandler>? _logger;

    public CrearComprobanteCommandHandler(
        INumeradorComprobanteService numerador,
        IComprobanteRepository repo,
        IUnitOfWorkFactory uowFactory,
        IGuiaVinculadaService guiaService,
        IParametrosService parametros,
        ILogger<CrearComprobanteCommandHandler>? logger = null)
    {
        _numerador = numerador;
        _repo = repo;
        _uowFactory = uowFactory;
        _guiaService = guiaService;
        _parametros = parametros;
        _logger = logger;
        _validator = new CrearComprobanteCommandValidator();
    }

    public async Task<Result<int?>> Handle(CrearComprobanteCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Validation — fail before any DB touches
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // Step 2: Determine origin
        OrigenComprobante origen = string.IsNullOrWhiteSpace(request.GuiaRef)
            ? new Standalone()
            : new DesdeGuia(request.GuiaRef);

        // Step 3: Numerador — master-DB, own connection (async)
        var numero = await _numerador.IncrementarYObtenerAsync(request.Tipo, cancellationToken);

        // Step 4: Get IGV rate (async)
        var igvRate = await _parametros.ObtenerIgvRateAsync(cancellationToken);

        // Step 5: Build aggregate from detalles
        var detalles = request.Detalles
            .Select(d => new DetalleComprobante(d.Cantidad, d.Descripcion, d.PrecioUnitario, d.Flete))
            .ToList();

        // Step 6: Build aggregate using factory method
        Agg comprobante = request.Tipo switch
        {
            TipoComprobante.Factura => Agg.CrearFactura(
                numero, DateTime.Today, request.ClienteCodigo,
                request.RucODni, request.Direccion, detalles, origen, igvRate),
            TipoComprobante.Boleta => Agg.CrearBoleta(
                numero, DateTime.Today, request.ClienteCodigo,
                request.RucODni, request.Direccion, detalles, origen),
            _ => throw new InvalidOperationException($"Unsupported TipoComprobante: {request.Tipo}")
        };

        // Step 7: Persist in yearly-DB Serializable transaction
        using var uow = _uowFactory.Create(request.Year);
        var codigo = _repo.Insertar(comprobante, uow);
        uow.Commit();

        // Step 8: Post-commit cross-aggregate write — only if DesdeGuia (best-effort)
        if (origen is DesdeGuia desdeGuia)
        {
            try
            {
                await _guiaService.VincularComprobanteAsync(desdeGuia.GuiaRef, comprobante.Numero, request.Year, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex,
                    "VincularComprobante failed for GuiaRef={GuiaRef} — continuing (best-effort).",
                    desdeGuia.GuiaRef);
            }
        }

        return Result<int?>.Ok(codigo);
    }
}
