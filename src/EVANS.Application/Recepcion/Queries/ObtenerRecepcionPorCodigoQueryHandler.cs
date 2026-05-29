using EVANS.Application.Common;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Application.Recepcion.Ports;
using MediatR;

namespace EVANS.Application.Recepcion.Queries;

public sealed class ObtenerRecepcionPorCodigoQueryHandler
    : IRequestHandler<ObtenerRecepcionPorCodigoQuery, Result<RecepcionDetalleDto?>>
{
    private readonly IRecepcionRepository _repo;

    public ObtenerRecepcionPorCodigoQueryHandler(IRecepcionRepository repo) => _repo = repo;

    public async Task<Result<RecepcionDetalleDto?>> Handle(
        ObtenerRecepcionPorCodigoQuery request, CancellationToken cancellationToken)
    {
        var recepcion = await _repo.ObtenerPorCodigoAsync(request.Codigo, request.Year, cancellationToken);

        if (recepcion is null)
            return Result<RecepcionDetalleDto?>.Ok(null);

        var detallesDto = recepcion.Detalles
            .Select(d => new DetalleRecepcionDto(
                d.Cantidad, d.Descripcion, d.Peso, d.Unidad, d.Costo, d.TipoDoc, d.NroDoc))
            .ToList()
            .AsReadOnly();

        var dto = new RecepcionDetalleDto(
            Codigo: recepcion.Codigo,
            FechaEmision: recepcion.FechaEmision,
            RemitenteCodigo: recepcion.RemitenteId,
            RemitenteNombre: string.Empty,     // enriched by infrastructure layer
            TipoDirPartida: recepcion.TipoDirPartida,
            DireccionPartida: recepcion.DireccionPartida,
            DestinatarioCodigo: recepcion.DestinatarioId,
            DestinatarioNombre: string.Empty,
            TipoDirDestino: recepcion.TipoDirDestino,
            DireccionDestino: recepcion.DireccionDestino,
            DestinoCodigo: recepcion.DestinoId,
            DestinoNombre: string.Empty,
            EstadoCodigo: recepcion.EstadoId,
            EstadoNombre: string.Empty,
            Bultos: recepcion.Bultos,
            PesoTotal: recepcion.PesoTotal,
            CostoTotal: recepcion.CostoTotal,
            GuiaRemisionVinculada: recepcion.GuiaRemisionVinculada,
            Observacion: recepcion.Observacion,
            UsuarioCodigo: recepcion.UsuarioId,
            Detalles: detallesDto);

        return Result<RecepcionDetalleDto?>.Ok(dto);
    }
}
