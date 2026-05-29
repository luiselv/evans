using EVANS.Application.GuiaRemision.Ports;
using EVANS.Application.Recepcion.DTOs;
using EVANS.Domain.Recepcion;

namespace EVANS.Application.Recepcion.Ports;

public interface IRecepcionRepository
{
    Task<int> CrearAsync(Domain.Recepcion.Recepcion recepcion, IUnitOfWork uow, CancellationToken ct);
    Task ActualizarAsync(Domain.Recepcion.Recepcion recepcion, IUnitOfWork uow, CancellationToken ct);
    Task EliminarAsync(int codigo, IUnitOfWork uow, CancellationToken ct);
    Task<Domain.Recepcion.Recepcion?> ObtenerPorCodigoAsync(int codigo, int year, CancellationToken ct);
    Task<IReadOnlyList<RecepcionListItemDto>> BuscarPorRangoFechasAsync(DateRange rango, int year, CancellationToken ct);
}
