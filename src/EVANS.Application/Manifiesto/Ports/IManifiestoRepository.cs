using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Manifiesto.DTOs;
using EVANS.Domain.Manifiesto;
using Agg = EVANS.Domain.Manifiesto.Manifiesto;

namespace EVANS.Application.Manifiesto.Ports;

public interface IManifiestoRepository
{
    Task<int> InsertarAsync(Agg manifiesto, IUnitOfWork uow, CancellationToken ct);
    Task ActualizarAsync(Agg manifiesto, IUnitOfWork uow, CancellationToken ct);
    Task EliminarAsync(int codigo, IUnitOfWork uow, CancellationToken ct);
    Task<ManifiestoDto?> ObtenerPorCodigoAsync(int codigo, int year, CancellationToken ct);
    Task<IReadOnlyList<ManifiestoResumenDto>> BuscarAsync(BuscarManifiestosFiltro filtro, int year, CancellationToken ct);
}
