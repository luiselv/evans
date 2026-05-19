using EVANS.Application.Common;
using EVANS.Application.Comprobante.Commands;
using EVANS.Application.Comprobante.Ports;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Shared.Ports;
using EVANS.Domain.Comprobante;
using MediatR;
using Agg = EVANS.Domain.Comprobante.Comprobante;

namespace EVANS.Application.Comprobante.Handlers;

public sealed class ActualizarComprobanteCommandHandler : IRequestHandler<ActualizarComprobanteCommand, Result<bool>>
{
    private readonly IComprobanteRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IParametrosService? _parametros;

    public ActualizarComprobanteCommandHandler(
        IComprobanteRepository repo,
        IUnitOfWorkFactory uowFactory,
        IParametrosService? parametros = null)
    {
        _repo = repo;
        _uowFactory = uowFactory;
        _parametros = parametros;
    }

    public async Task<Result<bool>> Handle(ActualizarComprobanteCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Load existing comprobante to enforce NumeroComprobante immutability (fiscal compliance).
        var existing = _repo.ObtenerPorCodigo(request.Codigo);
        if (existing is not null)
        {
            // Parse the stored "SERIE-NUMERO" format (e.g. "F001-000001") back to parts.
            var parts = existing.NumeroComprobante.Split('-', 2);
            var dbSerie  = parts.Length == 2 ? parts[0] : existing.NumeroComprobante;
            var dbNumero = parts.Length == 2 ? parts[1] : string.Empty;

            if (!string.Equals(request.Serie,  dbSerie,  StringComparison.Ordinal) ||
                !string.Equals(request.Numero, dbNumero, StringComparison.Ordinal))
            {
                throw new DomainException(
                    "NUMERO_INMUTABLE",
                    "El número de comprobante no puede modificarse.");
            }
        }

        // Step 2: Build updated detalles
        var detalles = request.Detalles
            .Select(d => new DetalleComprobante(d.Cantidad, d.Descripcion, d.PrecioUnitario, d.Flete))
            .ToList();

        // Step 3: Use command's serie/numero (already validated against DB above).
        var numero = new NumeroComprobante(request.Serie, request.Numero);

        // Step 4: Rebuild aggregate from command fields
        var igvRate = _parametros is not null
            ? await _parametros.ObtenerIgvRateAsync(cancellationToken)
            : 0.18m;

        Agg comprobante = request.Tipo switch
        {
            TipoComprobante.Factura => Agg.CrearFactura(
                numero, DateTime.Today, request.ClienteCodigo,
                request.RucODni, request.Direccion, detalles, new Standalone(), igvRate),
            TipoComprobante.Boleta => Agg.CrearBoleta(
                numero, DateTime.Today, request.ClienteCodigo,
                request.RucODni, request.Direccion, detalles, new Standalone()),
            _ => throw new InvalidOperationException($"Unsupported TipoComprobante: {request.Tipo}")
        };

        // Step 5: Set Codigo so repository knows which record to UPDATE
        comprobante.SetCodigo(request.Codigo);

        // Step 6: Persist in yearly-DB Serializable transaction
        using var uow = _uowFactory.Create(request.Year);
        _repo.Actualizar(comprobante, uow);
        uow.Commit();

        return Result<bool>.Ok(true);
    }
}
